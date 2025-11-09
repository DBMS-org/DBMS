using System.Globalization;

namespace Application.Utilities
{
    public static class SafeDataConverter
    {
        public static double? ParseDoubleOrNull(string? value, string fieldName = "", int lineNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (TryParseWithCulture(value, out double result))
                return result;

            Console.WriteLine($"Warning: Could not parse '{value}' as double for field '{fieldName}' at line {lineNumber}. Using null.");
            return null;
        }

        public static double ParseDoubleWithDefault(string? value, double defaultValue = 0.0, string fieldName = "", int lineNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            if (TryParseWithCulture(value, out double result))
                return result;

            Console.WriteLine($"Warning: Could not parse '{value}' as double for field '{fieldName}' at line {lineNumber}. Using default value {defaultValue}.");
            return defaultValue;
        }

        public static double ParseDoubleRequired(string? value, string fieldName, int lineNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException($"Required field '{fieldName}' is empty at line {lineNumber}");

            if (TryParseWithCulture(value, out double result))
                return result;

            throw new InvalidOperationException($"Cannot parse '{value}' as double for required field '{fieldName}' at line {lineNumber}");
        }

        public static int ParseIntWithDefault(string? value, int defaultValue = 0, string fieldName = "", int lineNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            var cleanValue = value.Split('.')[0].Trim();

            if (int.TryParse(cleanValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
                return result;

            if (int.TryParse(cleanValue, NumberStyles.Integer, CultureInfo.CurrentCulture, out result))
                return result;

            Console.WriteLine($"Warning: Could not parse '{value}' as integer for field '{fieldName}' at line {lineNumber}. Using default value {defaultValue}.");
            return defaultValue;
        }

        public static string ParseStringWithDefault(string? value, string defaultValue = "", bool allowEmpty = true)
        {
            if (string.IsNullOrWhiteSpace(value))
                return allowEmpty ? string.Empty : defaultValue;

            var cleaned = value.Trim();
            
            if ((cleaned.StartsWith("\"") && cleaned.EndsWith("\"")) ||
                (cleaned.StartsWith("'") && cleaned.EndsWith("'")))
            {
                cleaned = cleaned.Substring(1, cleaned.Length - 2);
            }

            return cleaned;
        }

        public static bool IsValidDrillingValue(double value, string fieldName)
        {
            return fieldName.ToLowerInvariant() switch
            {
                "easting" or "northing" => value >= -1000000 && value <= 10000000,
                "elevation" => value >= -1000 && value <= 10000,
                "length" or "depth" or "actualdepth" => value >= 0 && value <= 1000,
                "azimuth" => value >= 0 && value <= 360,
                "dip" => value >= -90 && value <= 90,
                "stemming" => value >= 0 && value <= 100,
                _ => true
            };
        }

        private static bool TryParseWithCulture(string value, out double result)
        {
            var cleanValue = value.Trim();
            
            cleanValue = System.Text.RegularExpressions.Regex.Replace(cleanValue, @"[^\d\.\-\+,]", "");

            if (double.TryParse(cleanValue, NumberStyles.Float | NumberStyles.AllowThousands,
                              CultureInfo.InvariantCulture, out result))
                return true;

            if (double.TryParse(cleanValue, NumberStyles.Float | NumberStyles.AllowThousands,
                              CultureInfo.CurrentCulture, out result))
                return true;

            var europeanCulture = new CultureInfo("de-DE");
            if (double.TryParse(cleanValue, NumberStyles.Float | NumberStyles.AllowThousands,
                              europeanCulture, out result))
                return true;

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

        public static T ParseEnumWithDefault<T>(string? value, T defaultValue, string fieldName = "", int lineNumber = 0) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            if (Enum.TryParse<T>(value, true, out T result))
                return result;

            Console.WriteLine($"Warning: Could not parse '{value}' as {typeof(T).Name} for field '{fieldName}' at line {lineNumber}. Using default value {defaultValue}.");
            return defaultValue;
        }

        public static T ParseEnumRequired<T>(string? value, string fieldName, int lineNumber = 0) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException($"Required enum field '{fieldName}' is empty at line {lineNumber}");

            if (Enum.TryParse<T>(value, true, out T result))
                return result;

            throw new InvalidOperationException($"Cannot parse '{value}' as {typeof(T).Name} for required field '{fieldName}' at line {lineNumber}");
        }

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

        public static string SafeToLower(string? value, string defaultValue = "")
        {
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value.ToLowerInvariant();
        }

        public static string SafeTrim(string? value, string defaultValue = "")
        {
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value.Trim();
        }

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