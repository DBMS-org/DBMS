using Domain.Entities;
using Application.Interfaces;
using Application.DTOs;
using Application.Utilities;
using System.Globalization;

namespace Application.Services
{
    public class DrillHoleApplicationService : IDrillHoleService
    {
        private readonly IDrillHoleRepository _repository;

        public DrillHoleApplicationService(IDrillHoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DrillHole>> GetAllDrillHolesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<DrillHole?> GetDrillHoleByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID cannot be null or empty", nameof(id));

            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<DrillHole>> GetDrillHolesByProjectIdAsync(int projectId)
        {
            return await _repository.GetByProjectIdAsync(projectId);
        }

        public async Task<IEnumerable<DrillHole>> GetDrillHolesBySiteIdAsync(int projectId, int siteId)
        {
            return await _repository.GetBySiteIdAsync(projectId, siteId);
        }

        public async Task<DrillHole> CreateDrillHoleAsync(DrillHole drillHole)
        {
            if (drillHole == null)
                throw new ArgumentNullException(nameof(drillHole));

            if (string.IsNullOrWhiteSpace(drillHole.Id))
                drillHole.Id = Guid.NewGuid().ToString();

            if (await _repository.ExistsAsync(drillHole.Id))
                throw new InvalidOperationException($"DrillHole with ID '{drillHole.Id}' already exists");

            ValidateDrillHole(drillHole);

            drillHole.CreatedAt = DateTime.UtcNow;
            drillHole.UpdatedAt = DateTime.UtcNow;

            return await _repository.AddAsync(drillHole);
        }

        public async Task UpdateDrillHoleAsync(DrillHole drillHole)
        {
            if (drillHole == null)
                throw new ArgumentNullException(nameof(drillHole));

            var existingDrillHole = await _repository.GetByIdAsync(drillHole.Id);
            if (existingDrillHole == null)
                throw new InvalidOperationException($"DrillHole with ID '{drillHole.Id}' not found");

            ValidateDrillHole(drillHole);

            drillHole.UpdatedAt = DateTime.UtcNow;
            drillHole.CreatedAt = existingDrillHole.CreatedAt; // Preserve original creation date

            await _repository.UpdateAsync(drillHole);
        }

        public async Task DeleteDrillHoleAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID cannot be null or empty", nameof(id));

            var existingDrillHole = await _repository.GetByIdAsync(id);
            if (existingDrillHole == null)
                throw new InvalidOperationException($"DrillHole with ID '{id}' not found");

            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<DrillHole>> CreateDrillHolesFromCsvAsync(CsvUploadRequest csvRequest)
        {
            if (csvRequest?.FileStream == null || csvRequest.FileSize == 0)
                throw new ArgumentException("File cannot be null or empty");

            // Clear existing drill holes before adding new ones from CSV
            await _repository.ClearAllAsync();

            var drillHoles = new List<DrillHole>();

            using var reader = new StreamReader(csvRequest.FileStream);
            
            string? line;
            var headers = new string[0];
            var mappedHeaders = new Dictionary<string, string>();
            var dataStarted = false;
            var lineNumber = 0;
            var has3DCapability = false;
            
            while ((line = await reader.ReadLineAsync()) != null)
            {
                lineNumber++;
                
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var values = line.Split(',').Select(v => v.Trim()).ToArray();
                
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
                        Console.WriteLine($"⚠️ Potential header at line {lineNumber} skipped – missing columns: {string.Join(", ", missingColumns)}");
                        continue;
                    }
                    
                    // Check if we have 3D capability
                    has3DCapability = CsvColumnMapper.Has3DCapability(mappedHeaders);
                    Console.WriteLine($"✅ Header found at line {lineNumber}. 3D capability: {has3DCapability}");
                    
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
                        drillHoles.Add(drillHole);
                    }
                }
                catch (Exception ex)
                {
                    // Log and skip invalid row instead of aborting entire upload
                    Console.WriteLine($"⚠️ Skipping line {lineNumber} due to parsing error: {ex.Message}");
                    continue;
                }
            }

            if (!dataStarted)
            {
                throw new InvalidOperationException("No valid CSV header row found containing required columns (ID, Easting, Northing, Elevation).");
            }

            if (drillHoles.Any())
            {
                await _repository.AddRangeAsync(drillHoles);
                Console.WriteLine($"Successfully imported {drillHoles.Count} drill holes. 3D data available: {has3DCapability}");
            }

            return drillHoles;
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
        private static DrillHole ParseFlexibleDrillHoleFromCsvLine(string[] headers, string[] values, Dictionary<string, string> mappedHeaders, int lineNumber)
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
                            Console.WriteLine($"Unknown standard column '{standardColumn}' at line {lineNumber}");
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
            ValidateDrillHole(drillHole);
            
            return drillHole;
        }

        private static DrillHole ParseBlastDrillHoleFromCsvLine(string[] headers, string[] values, int lineNumber)
        {
            var drillHole = new DrillHole
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            for (int i = 0; i < Math.Min(headers.Length, values.Length); i++)
            {
                var header = headers[i];
                var value = values[i];

                switch (header)
                {
                    case "sr no.":
                    case "sr no":
                    case "serial no.":
                    case "serial number":
                        break;
                    case "id":
                    case "hole id":
                    case "holeid":
                        drillHole.Id = string.IsNullOrWhiteSpace(value) ? Guid.NewGuid().ToString() : value;
                        drillHole.Name = drillHole.Id; // Use ID as name for blast holes
                        break;
                    case "east":
                    case "easting":
                    case "x":
                        drillHole.Easting = ParseDouble(value, "Easting", lineNumber);
                        break;
                    case "north":
                    case "northing":
                    case "y":
                        drillHole.Northing = ParseDouble(value, "Northing", lineNumber);
                        break;
                    case "elev":
                    case "elevation":
                    case "z":
                        drillHole.Elevation = ParseDouble(value, "Elevation", lineNumber);
                        break;
                    case "length":
                    case "hole length":
                        drillHole.Length = ParseDouble(value, "Length", lineNumber);
                        drillHole.Depth = drillHole.Length; // Set Depth same as Length for compatibility
                        break;
                    case "azi":
                    case "azimuth":
                    case "bearing":
                        drillHole.Azimuth = ParseDoubleNullable(value, "Azimuth", lineNumber);
                        break;
                    case "dip":
                    case "inclination":
                    case "angle":
                        drillHole.Dip = ParseDoubleNullable(value, "Dip", lineNumber);
                        break;
                    case "actual dep":
                    case "actualdep":
                    case "actual depth":
                    case "actual_depth":
                        drillHole.ActualDepth = ParseDouble(value, "Actual Depth", lineNumber);
                        break;
                    case "stemming":
                    case "stem":
                        drillHole.Stemming = ParseDouble(value, "Stemming", lineNumber);
                        break;
                    // Explicitly ignore explosive-related fields
                    case "total charg":
                    case "emulsion":
                    case "anfo":
                    case "total explosive mass":
                        // Ignore these fields as requested
                        break;
                    default:
                        // Ignore any other unknown fields
                        break;
                }
            }

            ValidateDrillHole(drillHole);
            return drillHole;
        }

        // Keep the original method for backward compatibility with standard CSV format
        private static DrillHole ParseDrillHoleFromCsvLine(string[] headers, string[] values, int lineNumber)
        {
            var drillHole = new DrillHole
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            for (int i = 0; i < headers.Length; i++)
            {
                var header = headers[i];
                var value = values[i];

                switch (header)
                {
                    case "id":
                        drillHole.Id = string.IsNullOrWhiteSpace(value) ? Guid.NewGuid().ToString() : value;
                        break;
                    case "name":
                        drillHole.Name = value ?? string.Empty;
                        break;
                    case "easting":
                        drillHole.Easting = ParseDouble(value, "Easting", lineNumber);
                        break;
                    case "northing":
                        drillHole.Northing = ParseDouble(value, "Northing", lineNumber);
                        break;
                    case "elevation":
                        drillHole.Elevation = ParseDouble(value, "Elevation", lineNumber);
                        break;
                    case "depth":
                        drillHole.Depth = ParseDouble(value, "Depth", lineNumber);
                        break;
                    case "azimuth":
                        drillHole.Azimuth = ParseDoubleNullable(value, "Azimuth", lineNumber);
                        break;
                    case "dip":
                        drillHole.Dip = ParseDoubleNullable(value, "Dip", lineNumber);
                        break;
                }
            }

            ValidateDrillHole(drillHole);
            return drillHole;
        }

        private static double ParseDouble(string value, string fieldName, int lineNumber)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0.0; // Default to 0 for empty values in blast data

            if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
                throw new InvalidOperationException($"Line {lineNumber}: Invalid {fieldName} value '{value}'");

            return result;
        }

        private static double? ParseDoubleNullable(string value, string fieldName, int lineNumber)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null; // Return null for empty values for optional 3D data

            if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double result))
            {
                Console.WriteLine($"Warning: Line {lineNumber}: Invalid {fieldName} value '{value}'. Setting to null for 2D fallback.");
                return null;
            }

            return result;
        }

        private static int ParseInt(string value, string fieldName, int lineNumber)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0; // Default to 0 for empty values in blast data

            if (!int.TryParse(value, out int result))
                throw new InvalidOperationException($"Line {lineNumber}: Invalid {fieldName} value '{value}'");

            return result;
        }

        private static void ValidateDrillHole(DrillHole drillHole)
        {
            if (string.IsNullOrWhiteSpace(drillHole.Id))
                throw new ArgumentException("DrillHole ID cannot be null or empty");

            if (string.IsNullOrWhiteSpace(drillHole.Name))
                drillHole.Name = drillHole.Id; // Use ID as name if name is empty

            if (drillHole.Depth < 0)
                throw new ArgumentException("DrillHole Depth cannot be negative");

            if (drillHole.Length < 0)
                throw new ArgumentException("DrillHole Length cannot be negative");

            if (drillHole.ActualDepth < 0)
                throw new ArgumentException("DrillHole Actual Depth cannot be negative");

            if (drillHole.Stemming < 0)
                throw new ArgumentException("DrillHole Stemming cannot be negative");

            // More lenient validation for blast hole data - handle nullable values
            if (drillHole.Azimuth.HasValue && (drillHole.Azimuth < 0 || drillHole.Azimuth > 360))
                drillHole.Azimuth = Math.Max(0, Math.Min(360, drillHole.Azimuth.Value)); // Clamp to valid range

            if (drillHole.Dip.HasValue && (drillHole.Dip < -90 || drillHole.Dip > 90))
                drillHole.Dip = Math.Max(-90, Math.Min(90, drillHole.Dip.Value)); // Clamp to valid range
        }

        public async Task DeleteDrillHolesByProjectIdAsync(int projectId)
        {
            await _repository.DeleteByProjectIdAsync(projectId);
        }

        public async Task DeleteDrillHolesBySiteIdAsync(int projectId, int siteId)
        {
            await _repository.DeleteBySiteIdAsync(projectId, siteId);
        }

        public async Task<int> GetDrillHoleCountAsync()
        {
            return await _repository.GetCountAsync();
        }

        public async Task<int> GetDrillHoleCountByProjectIdAsync(int projectId)
        {
            return await _repository.GetCountByProjectIdAsync(projectId);
        }

        public async Task<int> GetDrillHoleCountBySiteIdAsync(int projectId, int siteId)
        {
            return await _repository.GetCountBySiteIdAsync(projectId, siteId);
        }
    }
}
