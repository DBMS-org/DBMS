using Domain.Entities.DrillingOperations;
using Application.Interfaces.DrillingOperations;
using Application.DTOs.DrillingOperations;
using Application.DTOs.Shared;
using Application.Utilities;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Application.Services.DrillingOperations
{
    public class CsvImportApplicationService : ICsvImportService
    {
        private readonly IDrillHoleRepository _repository;
        private readonly IDrillHoleValidationService _validationService;
        private readonly ILogger<CsvImportApplicationService> _logger;

        // Resource management constants
        private const int BATCH_SIZE = 1000; // Process in batches for memory efficiency
        private const int MAX_FILE_SIZE_MB = 50; // Maximum file size in MB
        private const int OPERATION_TIMEOUT_MINUTES = 30; // Operation timeout

        public CsvImportApplicationService(
            IDrillHoleRepository repository,
            IDrillHoleValidationService validationService,
            ILogger<CsvImportApplicationService> logger)
        {
            _repository = repository;
            _validationService = validationService;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<DrillHole>>> CreateDrillHolesFromCsvAsync(
            CsvUploadRequest csvRequest, 
            CancellationToken cancellationToken = default)
        {
            // Input validation
            if (csvRequest?.FileStream == null || csvRequest.FileSize == 0)
            {
                _logger.LogError("CSV import failed: File cannot be null or empty");
                return Result.Failure<IEnumerable<DrillHole>>(ErrorCodes.Messages.ArgumentNull);
            }

            // File size validation
            if (csvRequest.FileSize > MAX_FILE_SIZE_MB * 1024 * 1024)
            {
                _logger.LogError("CSV import failed: File size {FileSize} MB exceeds maximum {MaxSize} MB", 
                    csvRequest.FileSize / (1024 * 1024), MAX_FILE_SIZE_MB);
                return Result.Failure<IEnumerable<DrillHole>>("File size exceeds maximum allowed size");
            }

            // Create operation timeout
            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromMinutes(OPERATION_TIMEOUT_MINUTES));
            using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

            _logger.LogInformation("Starting CSV import process for file size: {FileSize} bytes", csvRequest.FileSize);

            try
            {
                // Clear existing drill holes before adding new ones from CSV
                await _repository.ClearAllAsync();
                _logger.LogInformation("Cleared existing drill holes before CSV import");

                var allDrillHoles = new List<DrillHole>();

                // Use proper resource management with cancellation support
                using var reader = new StreamReader(csvRequest.FileStream);
                
                string? line;
                var headers = new string[0];
                var mappedHeaders = new Dictionary<string, string>();
                var dataStarted = false;
                var lineNumber = 0;
                var has3DCapability = false;
                var batchDrillHoles = new List<DrillHole>(BATCH_SIZE);
                
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    // Check for cancellation
                    combinedCts.Token.ThrowIfCancellationRequested();
                    
                    lineNumber++;
                    
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var values = SafeDataConverter.SafeSplit(line, ',');
                    
                    // Auto-detect header line - look for lines with common drill hole column indicators
                    if (!dataStarted && IsHeaderLine(values))
                    {
                        headers = values;
                        mappedHeaders = CsvColumnMapper.MapHeaders(headers);
                        
                        // Validate required columns
                        var (isValid, missingColumns) = CsvColumnMapper.ValidateRequiredColumns(mappedHeaders);
                        if (!isValid)
                        {
                            // Not a real header row, continue scanning
                            _logger.LogWarning("Potential header at line {LineNumber} skipped – missing columns: {MissingColumns}", 
                                lineNumber, string.Join(", ", missingColumns));
                            continue;
                        }
                        
                        // Check if we have 3D capability
                        has3DCapability = CsvColumnMapper.Has3DCapability(mappedHeaders);
                        _logger.LogInformation("Header found at line {LineNumber}. 3D capability: {Has3DCapability}", 
                            lineNumber, has3DCapability);
                        
                        dataStarted = true;
                        continue;
                    }
                    
                    // Skip lines before we find the header
                    if (!dataStarted)
                        continue;
                    
                    // Skip empty lines or rows without proper data
                    if (values.Length < 3 || IsDataRowEmpty(values, mappedHeaders))
                        continue;

                    try
                    {
                        var drillHole = ParseFlexibleDrillHoleFromCsvLine(headers, values, mappedHeaders, lineNumber);
                        
                        if (drillHole != null)
                        {
                            batchDrillHoles.Add(drillHole);
                            
                            // Process in batches for memory efficiency
                            if (batchDrillHoles.Count >= BATCH_SIZE)
                            {
                                await ProcessBatchAsync(batchDrillHoles, combinedCts.Token);
                                allDrillHoles.AddRange(batchDrillHoles);
                                batchDrillHoles.Clear();
                                
                                _logger.LogDebug("Processed batch of {BatchSize} drill holes, total: {TotalCount}", 
                                    BATCH_SIZE, allDrillHoles.Count);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log and skip invalid row instead of aborting entire upload
                        _logger.LogWarning("Skipping line {LineNumber} due to parsing error: {ErrorMessage}", 
                            lineNumber, ex.Message);
                        continue;
                    }
                }

                // Process remaining drill holes in the last batch
                if (batchDrillHoles.Any())
                {
                    await ProcessBatchAsync(batchDrillHoles, combinedCts.Token);
                    allDrillHoles.AddRange(batchDrillHoles);
                }

                if (!dataStarted)
                {
                    _logger.LogError("CSV import failed: No valid CSV header row found");
                    return Result.Failure<IEnumerable<DrillHole>>(ErrorCodes.Messages.InvalidCsvFormat);
                }

                if (allDrillHoles.Any())
                {
                    _logger.LogInformation("Successfully imported {DrillHoleCount} drill holes. 3D data available: {Has3DCapability}", 
                        allDrillHoles.Count, has3DCapability);
                }
                else
                {
                    _logger.LogWarning("CSV import completed but no drill holes were imported");
                }

                return Result.Success(allDrillHoles.AsEnumerable());
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("CSV import operation was cancelled");
                return Result.Failure<IEnumerable<DrillHole>>("Operation was cancelled");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during CSV import process");
                return Result.Failure<IEnumerable<DrillHole>>(ErrorCodes.Messages.InternalError);
            }
        }

        /// <summary>
        /// Processes a batch of drill holes with proper resource management
        /// </summary>
        private async Task ProcessBatchAsync(List<DrillHole> batchDrillHoles, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.AddRangeAsync(batchDrillHoles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing batch of {BatchSize} drill holes", batchDrillHoles.Count);
                throw; // Re-throw to be handled by the calling method
            }
        }

        /// <summary>
        /// Auto-detects if a line contains header information
        /// </summary>
        private static bool IsHeaderLine(string[] values)
        {
            if (values.Length < 3) return false;
            
            // Extended header indicators to cover more CSV formats
            var headerIndicators = new[] 
            { 
                "id", "hole", "east", "north", "elev", "elevation", "x", "y", "z",
                "azi", "azimuth", "dip", "length", "depth", "sr", "serial", "name",
                "collar", "design", "coord", "coordinate", "rl", "reduced", "level",
                "identifier", "mass", "diameter", "pattern", "assumed", "actual"
            };
            
            var normalizedValues = values.Select(v => v.ToLowerInvariant()).ToArray();

            // Check if at least 2 values contain header indicators
            var headerCount = normalizedValues.Count(v => headerIndicators.Any(indicator => v.Contains(indicator)));

            // Extra rule: if line contains any "design collar" columns, treat as header
            var containsDesignCollar = normalizedValues.Any(v => v.Contains("design collar"));

            return headerCount >= 2 || containsDesignCollar;
        }

        /// <summary>
        /// Checks if a data row is empty or invalid
        /// </summary>
        private static bool IsDataRowEmpty(string[] values, Dictionary<string, string> mappedHeaders)
        {
            // If no mappings available, check basic criteria
            if (!mappedHeaders.Any())
            {
                // Count non-empty values
                var nonEmptyCount = values.Count(v => !string.IsNullOrWhiteSpace(v));
                return nonEmptyCount < 3; // Need at least 3 non-empty values
            }

            // Find the ID column and check if it has data
            var idColumn = mappedHeaders.FirstOrDefault(kv => kv.Value == "id");
            if (idColumn.Key != null)
            {
                // We need to find the index in the original headers array, not the mapped headers
                // Since we don't have access to the headers array here, let's use a simpler approach
                var requiredValues = new List<string>();
                
                // Check if we have values for essential columns
                foreach (var mapping in mappedHeaders)
                {
                    if (mapping.Value == "id" || mapping.Value == "easting" || mapping.Value == "northing")
                    {
                        // Find the corresponding index - this is a simplified approach
                        var headersList = mappedHeaders.Keys.ToList();
                        var index = headersList.IndexOf(mapping.Key);
                        if (index >= 0 && index < values.Length)
                        {
                            requiredValues.Add(values[index]);
                        }
                    }
                }
                
                // Return true if any essential value is missing
                return requiredValues.Any(string.IsNullOrWhiteSpace);
            }
            
            // Fallback: check if first few columns are empty
            return values.Take(3).All(string.IsNullOrWhiteSpace);
        }

        /// <summary>
        /// Flexible CSV parsing that works with any column format using the mapping system
        /// </summary>
        private DrillHole ParseFlexibleDrillHoleFromCsvLine(string[] headers, string[] values, Dictionary<string, string> mappedHeaders, int lineNumber)
        {
            var drillHole = new DrillHole
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Parse each column using the mapped headers
            for (int i = 0; i < Math.Min(headers.Length, values.Length); i++)
            {
                var originalHeader = headers[i];
                var value = values[i];
                var standardColumn = CsvColumnMapper.GetStandardColumnName(originalHeader, mappedHeaders);

                if (string.IsNullOrEmpty(standardColumn))
                    continue; // Skip unmapped columns

                try
                {
                    switch (standardColumn)
                    {
                        case "id":
                            drillHole.Id = SafeDataConverter.ParseStringWithDefault(value, Guid.NewGuid().ToString());
                            if (string.IsNullOrWhiteSpace(drillHole.Name)) // Use ID as name if name not set
                                drillHole.Name = drillHole.Id;
                            break;

                        case "name":
                            drillHole.Name = SafeDataConverter.ParseStringWithDefault(value, drillHole.Id);
                            break;

                        case "easting":
                            drillHole.Easting = SafeDataConverter.ParseDoubleRequired(value, "Easting", lineNumber);
                            break;

                        case "northing":
                            drillHole.Northing = SafeDataConverter.ParseDoubleRequired(value, "Northing", lineNumber);
                            break;

                        case "elevation":
                            drillHole.Elevation = SafeDataConverter.ParseDoubleRequired(value, "Elevation", lineNumber);
                            break;

                        case "length":
                            drillHole.Length = SafeDataConverter.ParseDoubleWithDefault(value, 0.0, "Length", lineNumber);
                            // If depth is not separately provided, use length as depth
                            if (drillHole.Depth == 0)
                                drillHole.Depth = drillHole.Length;
                            break;

                        case "depth":
                            drillHole.Depth = SafeDataConverter.ParseDoubleWithDefault(value, 0.0, "Depth", lineNumber);
                            break;

                        case "azimuth":
                            drillHole.Azimuth = SafeDataConverter.ParseDoubleOrNull(value, "Azimuth", lineNumber);
                            break;

                        case "dip":
                            drillHole.Dip = SafeDataConverter.ParseDoubleOrNull(value, "Dip", lineNumber);
                            break;

                        case "stemming":
                            drillHole.Stemming = SafeDataConverter.ParseDoubleWithDefault(value, 0.0, "Stemming", lineNumber);
                            break;

                        default:
                            // Handle any other mapped columns that might be added in the future
                            _logger.LogDebug("Unknown standard column '{StandardColumn}' at line {LineNumber}", 
                                standardColumn, lineNumber);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error parsing '{standardColumn}' from value '{value}' at line {lineNumber}: {ex.Message}");
                }
            }

            // Set default values if required fields are missing
            if (string.IsNullOrWhiteSpace(drillHole.Id))
                drillHole.Id = Guid.NewGuid().ToString();
                
            if (string.IsNullOrWhiteSpace(drillHole.Name))
                drillHole.Name = drillHole.Id;

            // Validate coordinates
            try
            {
                (drillHole.Easting, drillHole.Northing, drillHole.Elevation) = 
                    SafeDataConverter.ValidateCoordinates(drillHole.Easting, drillHole.Northing, drillHole.Elevation, lineNumber);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Invalid coordinates at line {lineNumber}: {ex.Message}");
            }

            // Validate 3D data if present
            (drillHole.Azimuth, drillHole.Dip) = SafeDataConverter.ValidateOptional3DData(drillHole.Azimuth, drillHole.Dip, lineNumber);

            // Final validation
            _validationService.ValidateDrillHole(drillHole);
            
            return drillHole;
        }
    }
} 