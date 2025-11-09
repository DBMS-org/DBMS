using System.Globalization;

namespace Application.Utilities
{
    public static class SafeDataConverter
    {
        /// <summary>
        /// Safely parses a string value to double with fallback to null for optional fields
        /// </summary>
        public static double? ParseDoubleOrNull(string? value, string fieldName = "", int lineNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            // Try to parse with different culture settings
            if (TryParseWithCulture(value, out double result))
                return result;

            // Log warning but don't throw exception for optional fields
            Console.WriteLine($"Warning: Could not parse '{value}' as double for field '{fieldName}' at line {lineNumber}. Using null.");
            return null;
        }

        /// <summary>
        /// Safely parses a string value to double with fallback to default value for required fields
        /// </summary>
        public static double ParseDoubleWithDefault(string? value, double defaultValue = 0.0, string fieldName = "", int lineNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            // Try to parse with different culture settings
            if (TryParseWithCulture(value, out double result))
                return result;

            // For required fields, log warning and use default
            Console.WriteLine($"Warning: Could not parse '{value}' as double for field '{fieldName}' at line {lineNumber}. Using default value {defaultValue}.");
            return defaultValue;
        }

        /// <summary>
        /// Safely parses a string value to double, throws exception for required fields if parsing fails
        /// </summary>
        public static double ParseDoubleRequired(string? value, string fieldName, int lineNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException($"Required field '{fieldName}' is empty at line {lineNumber}");

            // Try to parse with different culture settings
            if (TryParseWithCulture(value, out double result))
                return result;

            throw new InvalidOperationException($"Cannot parse '{value}' as double for required field '{fieldName}' at line {lineNumber}");
        }

        /// <summary>
        /// Safely parses a string value to integer with fallback to default value
        /// </summary>
        public static int ParseIntWithDefault(string? value, int defaultValue = 0, string fieldName = "", int lineNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            // Remove any decimal part if present (e.g., "123.0" -> "123")
            var cleanValue = value.Split('.')[0].Trim();

            if (int.TryParse(cleanValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
                return result;

            // Try with current culture
            if (int.TryParse(cleanValue, NumberStyles.Integer, CultureInfo.CurrentCulture, out result))
                return result;

            Console.WriteLine($"Warning: Could not parse '{value}' as integer for field '{fieldName}' at line {lineNumber}. Using default value {defaultValue}.");
            return defaultValue;
        }

        /// <summary>
        /// Safely trims and cleans string values
        /// </summary>
        public static string ParseStringWithDefault(string? value, string defaultValue = "", bool allowEmpty = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return allowEmpty ? string.Empty : defaultValue;

            var cleaned = value.Trim();
            
            // Remove quotes if present
            if ((cleaned.StartsWith("\"") && cleaned.EndsWith("\"")) ||
                (cleaned.StartsWith("'") && cleaned.EndsWith("'")))
            {
                cleaned = cleaned.Substring(1, cleaned.Length - 2);
            }

            return cleaned;
        }

        /// <summary>
        /// Validates that a numeric value is within reasonable bounds for drilling data
        /// </summary>
        public static bool IsValidDrillingValue(double value, string fieldName)
        {
            return fieldName.ToLowerInvariant() switch
            {
                "easting" or "northing" => value >= -1000000 && value <= 10000000, // Reasonable coordinate range
                "elevation" => value >= -1000 && value <= 10000, // Reasonable elevation range
                "length" or "depth" or "actualdepth" => value >= 0 && value <= 1000, // Reasonable drilling depth
                "azimuth" => value >= 0 && value <= 360, // Valid azimuth range
                "dip" => value >= -90 && value <= 90, // Valid dip range
                "stemming" => value >= 0 && value <= 100, // Reasonable stemming range
                _ => true // Allow any value for unknown fields
            };
        }

        /// <summary>
        /// Attempts to parse double with multiple culture formats
        /// </summary>
        private static bool TryParseWithCulture(string value, out double result)
        {
            // Clean the value
            var cleanValue = value.Trim();
            
            // Remove any currency symbols or other common prefixes/suffixes
            cleanValue = System.Text.RegularExpressions.Regex.Replace(cleanValue, @"[^\d\.\-\+,]", "");

            // Try with invariant culture first (most common)
            if (double.TryParse(cleanValue, NumberStyles.Float | NumberStyles.AllowThousands, 
                              CultureInfo.InvariantCulture, out result))
                return true;

            // Try with current culture
            if (double.TryParse(cleanValue, NumberStyles.Float | NumberStyles.AllowThousands, 
                              CultureInfo.CurrentCulture, out result))
                return true;

            // Try with common European format (comma as decimal separator)
            var europeanCulture = new CultureInfo("de-DE");
            if (double.TryParse(cleanValue, NumberStyles.Float | NumberStyles.AllowThousands, 
                              europeanCulture, out result))
                return true;

            // Try replacing comma with dot and vice versa
            if (cleanValue.Contains(','))
            {
                var dotVersion = cleanValue.Replace(',', '.');
                if (double.TryParse(dotVersion, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
                    return true;
            }

            if (cleanValue.Contains('.'))
            {
                var commaVersion = cleanValue.Replace('.', ',');
                if (double.TryParse(commaVersion, NumberStyles.Float, europeanCulture, out result))
                    return true;
            }

            result = 0;
            return false;
        }

        /// <summary>
        /// Validates and normalizes coordinate values
        /// </summary>
        public static (double easting, double northing, double elevation) ValidateCoordinates(
            double easting, double northing, double elevation, int lineNumber = 0)
        {
            if (!IsValidDrillingValue(easting, "easting"))
                throw new InvalidOperationException($"Invalid easting value {easting} at line {lineNumber}");

            if (!IsValidDrillingValue(northing, "northing"))
                throw new InvalidOperationException($"Invalid northing value {northing} at line {lineNumber}");

            if (!IsValidDrillingValue(elevation, "elevation"))
                throw new InvalidOperationException($"Invalid elevation value {elevation} at line {lineNumber}");

            return (easting, northing, elevation);
        }

        /// <summary>
        /// Validates azimuth and dip values if provided
        /// </summary>
        public static (double? azimuth, double? dip) ValidateOptional3DData(
            double? azimuth, double? dip, int lineNumber = 0)
        {
            if (azimuth.HasValue && !IsValidDrillingValue(azimuth.Value, "azimuth"))
            {
                Console.WriteLine($"Warning: Invalid azimuth value {azimuth} at line {lineNumber}. Setting to null.");
                azimuth = null;
            }

            if (dip.HasValue && !IsValidDrillingValue(dip.Value, "dip"))
            {
                Console.WriteLine($"Warning: Invalid dip value {dip} at line {lineNumber}. Setting to null.");
                dip = null;
            }

            return (azimuth, dip);
        }

        /// <summary>
        /// Safely parses enum values with fallback to default
        /// </summary>
        public static T ParseEnumWithDefault<T>(string? value, T defaultValue, string fieldName = "", int lineNumber = 0) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            if (Enum.TryParse<T>(value, true, out T result))
                return result;

            Console.WriteLine($"Warning: Could not parse '{value}' as {typeof(T).Name} for field '{fieldName}' at line {lineNumber}. Using default value {defaultValue}.");
            return defaultValue;
        }

        /// <summary>
        /// Safely parses enum values, throws exception if parsing fails
        /// </summary>
        public static T ParseEnumRequired<T>(string? value, string fieldName, int lineNumber = 0) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException($"Required enum field '{fieldName}' is empty at line {lineNumber}");

            if (Enum.TryParse<T>(value, true, out T result))
                return result;

            throw new InvalidOperationException($"Cannot parse '{value}' as {typeof(T).Name} for required field '{fieldName}' at line {lineNumber}");
        }

        /// <summary>
        /// Safely extracts numeric ID from string with prefix (e.g., "DH123" -> 123)
        /// </summary>
        public static int ExtractIdFromString(string? value, string prefix, int defaultValue = 0, string fieldName = "", int lineNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            if (!value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Warning: Value '{value}' does not start with expected prefix '{prefix}' for field '{fieldName}' at line {lineNumber}. Using default value {defaultValue}.");
                return defaultValue;
            }

            var numericPart = value.Substring(prefix.Length);
            if (int.TryParse(numericPart, out int result))
                return result;

            Console.WriteLine($"Warning: Could not extract numeric ID from '{value}' for field '{fieldName}' at line {lineNumber}. Using default value {defaultValue}.");
            return defaultValue;
        }

        /// <summary>
        /// Validates that a string is not null or empty, with better error messages
        /// </summary>
        public static bool IsValidString(string? value, bool allowEmpty = false, string fieldName = "", int lineNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (!allowEmpty)
                {
                    Console.WriteLine($"Warning: Field '{fieldName}' is empty at line {lineNumber}");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Safely converts string to lowercase with null safety
        /// </summary>
        public static string SafeToLower(string? value, string defaultValue = "")
        {
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value.ToLowerInvariant();
        }

        /// <summary>
        /// Safely trims string with null safety
        /// </summary>
        public static string SafeTrim(string? value, string defaultValue = "")
        {
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value.Trim();
        }

        /// <summary>
        /// Safely splits string with null safety and trimming
        /// </summary>
        public static string[] SafeSplit(string? value, char separator = ',', bool removeEmptyEntries = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Array.Empty<string>();

            var options = removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            return value.Split(separator, options)
                       .Select(v => v.Trim())
                       .ToArray();
        }
    }
} 