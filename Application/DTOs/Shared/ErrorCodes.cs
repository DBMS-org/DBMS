namespace Application.DTOs.Shared;

// Centralized error codes and messages
public static class ErrorCodes
{
    public const string NotFound = "RESOURCE_NOT_FOUND";
    public const string AlreadyExists = "RESOURCE_ALREADY_EXISTS";
    public const string ValidationFailed = "VALIDATION_FAILED";
    public const string Unauthorized = "UNAUTHORIZED";
    public const string Forbidden = "FORBIDDEN";
    public const string InternalError = "INTERNAL_ERROR";
    public const string InvalidOperation = "INVALID_OPERATION";
    public const string ArgumentNull = "ARGUMENT_NULL";
    public const string ArgumentInvalid = "ARGUMENT_INVALID";

    public const string UserNotFound = "USER_NOT_FOUND";
    public const string UserAlreadyExists = "USER_ALREADY_EXISTS";
    public const string InvalidCredentials = "INVALID_CREDENTIALS";
    public const string WeakPassword = "WEAK_PASSWORD";
    public const string InvalidEmail = "INVALID_EMAIL";

    public const string ProjectNotFound = "PROJECT_NOT_FOUND";
    public const string ProjectSiteNotFound = "PROJECT_SITE_NOT_FOUND";
    public const string InvalidProjectOwnership = "INVALID_PROJECT_OWNERSHIP";

    public const string MachineNotFound = "MACHINE_NOT_FOUND";
    public const string MachineAlreadyAssigned = "MACHINE_ALREADY_ASSIGNED";
    public const string MachineNotAvailable = "MACHINE_NOT_AVAILABLE";

    public const string DrillHoleNotFound = "DRILL_HOLE_NOT_FOUND";
    public const string DrillHoleAlreadyExists = "DRILL_HOLE_ALREADY_EXISTS";
    public const string InvalidCoordinates = "INVALID_COORDINATES";
    public const string DuplicateCoordinates = "DUPLICATE_COORDINATES";
    public const string MaxDrillPointsExceeded = "MAX_DRILL_POINTS_EXCEEDED";
    public const string DrillPointNotFound = "DRILL_POINT_NOT_FOUND";
    public const string CsvParsingError = "CSV_PARSING_ERROR";
    public const string CsvValidationError = "CSV_VALIDATION_ERROR";
    public const string InvalidCsvFormat = "INVALID_CSV_FORMAT";

    public const string DrillPatternNotFound = "DRILL_PATTERN_NOT_FOUND";
    public const string InvalidDrillPatternOwnership = "INVALID_DRILL_PATTERN_OWNERSHIP";

    public static class Messages
    {
        // General Messages
        public const string ResourceNotFound = "The requested resource could not be found. Please verify the information and try again.";
        public const string ResourceAlreadyExists = "This resource already exists in the system. Please use a different identifier.";
        public const string ValidationFailed = "Please review the form and correct the highlighted errors before submitting.";
        public const string Unauthorized = "You need to be logged in to perform this action. Please log in and try again.";
        public const string Forbidden = "You don't have permission to perform this action. Please contact your administrator if you need access.";
        public const string InternalError = "An unexpected error occurred. Our team has been notified and is working to resolve the issue.";
        public const string InvalidOperation = "This operation cannot be performed at this time. Please check the current state and try again.";
        public const string ArgumentNull = "Required information is missing. Please provide all required fields.";
        public const string ArgumentInvalid = "Some of the provided information is invalid. Please check your input and try again.";
        public const string InvalidCsvFormat = "The CSV file format is invalid or missing required columns. Please ensure your file follows the correct template.";

        // User Management Messages
        public static string UserNotFound(int id) => $"User with ID {id} could not be found. The user may have been deleted or the ID is incorrect.";
        public static string UserNotFound(string identifier) => $"User '{identifier}' could not be found. Please verify the username or email and try again.";
        public static string UserAlreadyExists(string email) => $"A user with email '{email}' already exists. Please use a different email address.";

        // Project Management Messages
        public static string ProjectNotFound(int id) => $"Project with ID {id} could not be found. It may have been deleted or archived.";
        public static string ProjectSiteNotFound(int id) => $"Project site with ID {id} could not be found. Please verify the site ID and try again.";

        // Machine Management Messages
        public static string MachineNotFound(int id) => $"Machine with ID {id} could not be found. The machine may have been removed from the system.";

        // Drilling Operations Messages
        public static string DrillHoleNotFound(string id) => $"Drill hole '{id}' could not be found. Please verify the drill hole ID.";
        public static string DrillHoleAlreadyExists(string id) => $"A drill hole with ID '{id}' already exists at this location. Please use a different identifier.";
        public static string DrillPatternNotFound(int id) => $"Drill pattern with ID {id} could not be found. It may have been deleted.";
        public static string InvalidCoordinates(double x, double y) => $"The coordinates ({x}, {y}) are invalid. Please ensure coordinates are within the permitted range.";
        public static string DuplicateCoordinates(double x, double y) => $"A drill point already exists at coordinates ({x:F2}, {y:F2}). Please choose a different location.";
        public static string MaxDrillPointsExceeded(int maxPoints) => $"You have exceeded the maximum number of drill points ({maxPoints}). Please remove some points before adding more.";
        public static string DrillPointNotFound(string id) => $"Drill point '{id}' could not be found in the pattern.";

        // CSV Import Messages
        public static string CsvParsingError(int line, string error) => $"Error reading CSV file at line {line}: {error}. Please check the file format and try again.";
        public static string CsvValidationError(string field, string value) => $"Invalid value '{value}' for field '{field}'. Please correct this value and re-upload the file.";
    }
} 