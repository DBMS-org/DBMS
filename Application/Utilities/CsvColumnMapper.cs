using System.Text.RegularExpressions;

namespace Application.Utilities
{
    public static class CsvColumnMapper
    {
        // Column mapping dictionary with all possible variations
        private static readonly Dictionary<string, List<string>> ColumnMappings = new()
        {
            ["id"] = new List<string> 
            { 
                "id", "hole id", "holeid", "hole_id", "holeidentifier", "hole identifier", 
                "sr no", "sr no.", "serial no", "serial no.", "serial number", "serialnumber" 
            },
            ["name"] = new List<string> 
            { 
                "name", "hole name", "holename", "hole_name", "label", "description", 
                "drill pattern (name)", "drill pattern name", "drill pattern", "pattern", "pattern name" 
            },
            ["easting"] = new List<string> 
            { 
                "easting", "east", "x", "x coord", "x coordinate", "x_coord", "x_coordinate",
                "design collar (e)", "design collar e", "designcollare", "collar east",
                "collar easting", "collareasting", "collar_east", "collar_easting"
            },
            ["northing"] = new List<string> 
            { 
                "northing", "north", "y", "y coord", "y coordinate", "y_coord", "y_coordinate",
                "design collar (n)", "design collar n", "designcollarn", "collar north",
                "collar northing", "collarnorthing", "collar_north", "collar_northing"
            },
            ["elevation"] = new List<string> 
            { 
                "elevation", "elev", "z", "z coord", "z coordinate", "z_coord", "z_coordinate",
                "design collar (rl)", "design collar rl", "designcollarrl", "collar elevation",
                "collarelevation", "collar_elevation", "rl", "reduced level"
            },
            ["length"] = new List<string> 
            { 
                "length", "hole length", "holelength", "hole_length", "total length",
                "totallength", "total_length", "planned length", "plannedlength", "planned_length"
            },
            ["depth"] = new List<string> 
            { 
                "depth", "hole depth", "holedepth", "hole_depth", "actual depth", 
                "actualdepth", "actual_depth", "final depth", "finaldepth", "final_depth",
                "actual dep", "actualdep", "actual_dep", "assumed depth", "assumeddepth", "assumed_depth"
            },
            ["azimuth"] = new List<string> 
            { 
                "azimuth", "azi", "bearing", "direction", "angle", "orientation",
                "az", "azimuth angle", "azimuthangle", "azimuth_angle"
            },
            ["dip"] = new List<string> 
            { 
                "dip", "inclination", "inc", "angle", "slope", "gradient",
                "dip angle", "dipangle", "dip_angle", "inclination angle",
                "inclinationangle", "inclination_angle"
            },
            ["stemming"] = new List<string> 
            { 
                "stemming", "stem", "stemming length", "stemminglength", "stemming_length",
                "stem length", "stemlength", "stem_length"
            }
        };

        // Required columns for basic drill hole data (2D minimum)
        private static readonly List<string> RequiredColumns = new() 
        { 
            "id", "easting", "northing", "elevation"
        };

        // Optional columns that provide additional functionality
        private static readonly List<string> OptionalColumns = new() 
        { 
            "name", "length", "depth", "azimuth", "dip", "stemming"
        };

        /// <summary>
        /// Normalizes a header string by removing special characters, spaces, and converting to lowercase
        /// </summary>
        public static string NormalizeHeader(string header)
        {
            if (string.IsNullOrWhiteSpace(header))
                return string.Empty;

            // Remove parentheses, brackets, and common punctuation
            string normalized = Regex.Replace(header, @"[()[\]{}]", "");
            
            // Replace multiple spaces/underscores/dashes with single space
            normalized = Regex.Replace(normalized, @"[\s_-]+", " ");
            
            // Trim and convert to lowercase
            normalized = normalized.Trim().ToLowerInvariant();
            
            return normalized;
        }

        /// <summary>
        /// Maps CSV headers to standardized column names
        /// </summary>
        public static Dictionary<string, string> MapHeaders(string[] headers)
        {
            var mappedHeaders = new Dictionary<string, string>();
            var normalizedHeaders = headers.Select(NormalizeHeader).ToArray();

            foreach (var standardColumn in ColumnMappings.Keys)
            {
                var aliases = ColumnMappings[standardColumn];
                
                for (int i = 0; i < normalizedHeaders.Length; i++)
                {
                    var normalizedHeader = normalizedHeaders[i];
                    
                    // Check if this header matches any of the aliases for this standard column
                    if (aliases.Any(alias => normalizedHeader.Equals(alias, StringComparison.OrdinalIgnoreCase)))
                    {
                        mappedHeaders[headers[i]] = standardColumn; // Store original header as key
                        break; // Found a match, move to next standard column
                    }
                }
            }

            return mappedHeaders;
        }

        /// <summary>
        /// Validates that required columns are present in the mapped headers
        /// </summary>
        public static (bool IsValid, List<string> MissingColumns) ValidateRequiredColumns(Dictionary<string, string> mappedHeaders)
        {
            var missingColumns = new List<string>();
            var mappedStandardColumns = mappedHeaders.Values.ToHashSet();

            foreach (var requiredColumn in RequiredColumns)
            {
                if (!mappedStandardColumns.Contains(requiredColumn))
                {
                    missingColumns.Add(requiredColumn);
                }
            }

            return (missingColumns.Count == 0, missingColumns);
        }

        /// <summary>
        /// Gets the standardized column name for a given original header
        /// </summary>
        public static string? GetStandardColumnName(string originalHeader, Dictionary<string, string> mappedHeaders)
        {
            return mappedHeaders.TryGetValue(originalHeader, out var standardName) ? standardName : null;
        }

        /// <summary>
        /// Checks if the CSV has sufficient data for 3D visualization
        /// </summary>
        public static bool Has3DCapability(Dictionary<string, string> mappedHeaders)
        {
            var mappedStandardColumns = mappedHeaders.Values.ToHashSet();
            return mappedStandardColumns.Contains("azimuth") && mappedStandardColumns.Contains("dip");
        }

        /// <summary>
        /// Gets all possible column variations for documentation/help purposes
        /// </summary>
        public static Dictionary<string, List<string>> GetAllColumnVariations()
        {
            return new Dictionary<string, List<string>>(ColumnMappings);
        }
    }
} 