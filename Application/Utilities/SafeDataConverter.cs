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
    }
} 