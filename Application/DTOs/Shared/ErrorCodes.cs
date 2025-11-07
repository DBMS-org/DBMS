namespace Application.DTOs.Shared;

/// <summary>
/// Centralized error codes and messages for consistent error handling
/// </summary>
public static class ErrorCodes
{
    // General errors
    public const string NotFound = "RESOURCE_NOT_FOUND";
    public const string AlreadyExists = "RESOURCE_ALREADY_EXISTS";
    public const string ValidationFailed = "VALIDATION_FAILED";
    public const string Unauthorized = "UNAUTHORIZED";
    public const string Forbidden = "FORBIDDEN";
    public const string InternalError = "INTERNAL_ERROR";
    public const string InvalidOperation = "INVALID_OPERATION";
    public const string ArgumentNull = "ARGUMENT_NULL";
    public const string ArgumentInvalid = "ARGUMENT_INVALID";

    // User management errors
    public const string UserNotFound = "USER_NOT_FOUND";
    public const string UserAlreadyExists = "USER_ALREADY_EXISTS";
    public const string InvalidCredentials = "INVALID_CREDENTIALS";
    public const string WeakPassword = "WEAK_PASSWORD";
    public const string InvalidEmail = "INVALID_EMAIL";

    // Project management errors
    public const string ProjectNotFound = "PROJECT_NOT_FOUND";
    public const string ProjectSiteNotFound = "PROJECT_SITE_NOT_FOUND";
    public const string InvalidProjectOwnership = "INVALID_PROJECT_OWNERSHIP";

    // Machine management errors
    public const string MachineNotFound = "MACHINE_NOT_FOUND";
    public const string MachineAlreadyAssigned = "MACHINE_ALREADY_ASSIGNED";
    public const string MachineNotAvailable = "MACHINE_NOT_AVAILABLE";

    // Drilling operations errors
    public const string DrillHoleNotFound = "DRILL_HOLE_NOT_FOUND";
    public const string DrillHoleAlreadyExists = "DRILL_HOLE_ALREADY_EXISTS";
    public const string InvalidCoordinates = "INVALID_COORDINATES";
    public const string DuplicateCoordinates = "DUPLICATE_COORDINATES";
    public const string MaxDrillPointsExceeded = "MAX_DRILL_POINTS_EXCEEDED";
    public const string DrillPointNotFound = "DRILL_POINT_NOT_FOUND";
    public const string CsvParsingError = "CSV_PARSING_ERROR";
    public const string CsvValidationError = "CSV_VALIDATION_ERROR";
    public const string InvalidCsvFormat = "INVALID_CSV_FORMAT";

    // Blasting operations errors
    public const string DrillPatternNotFound = "DRILL_PATTERN_NOT_FOUND";
    public const string InvalidDrillPatternOwnership = "INVALID_DRILL_PATTERN_OWNERSHIP";

    // Error messages
    public static class Messages
    {
        public const string ResourceNotFound = "The requested resource was not found.";
        public const string ResourceAlreadyExists = "The resource already exists.";
        public const string ValidationFailed = "Validation failed for the provided data.";
        public const string Unauthorized = "You are not authorized to perform this action.";
        public const string Forbidden = "Access to this resource is forbidden.";
        public const string InternalError = "An internal error occurred while processing your request.";
        public const string InvalidOperation = "The requested operation is not valid.";
        public const string ArgumentNull = "A required argument was null or empty.";
        public const string ArgumentInvalid = "One or more arguments are invalid.";
        public const string InvalidCsvFormat = "The CSV file format is invalid or missing required columns.";

        public static string UserNotFound(int id) => $"User with ID {id} was not found.";
        public static string UserNotFound(string identifier) => $"User with identifier '{identifier}' was not found.";
        public static string UserAlreadyExists(string email) => $"User with email '{email}' already exists.";
        public static string ProjectNotFound(int id) => $"Project with ID {id} was not found.";
        public static string ProjectSiteNotFound(int id) => $"Project site with ID {id} was not found.";
        public static string MachineNotFound(int id) => $"Machine with ID {id} was not found.";
        public static string DrillHoleNotFound(string id) => $"Drill hole with ID '{id}' was not found.";
        public static string DrillHoleAlreadyExists(string id) => $"Drill hole with ID '{id}' already exists.";

        public static string DrillPatternNotFound(int id) => $"Drill pattern with ID {id} was not found.";
        public static string InvalidCoordinates(double x, double y) => $"Invalid coordinates: ({x}, {y}).";
        public static string DuplicateCoordinates(double x, double y) => $"A drill point already exists at coordinates ({x:F2}, {y:F2}).";
        public static string MaxDrillPointsExceeded(int maxPoints) => $"Maximum number of drill points ({maxPoints}) exceeded.";
        public static string DrillPointNotFound(string id) => $"Drill point with ID '{id}' was not found.";
        public static string CsvParsingError(int line, string error) => $"CSV parsing error at line {line}: {error}";
        public static string CsvValidationError(string field, string value) => $"CSV validation error for field '{field}' with value '{value}'.";
    }
} 