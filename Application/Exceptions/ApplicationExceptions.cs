namespace Application.Exceptions
{
    /// <summary>
    /// Base exception for all application layer exceptions
    /// </summary>
    public abstract class ApplicationException : Exception
    {
        protected ApplicationException(string message) : base(message) { }
        protected ApplicationException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Exception thrown when input validation fails
    /// </summary>
    public class ValidationException : ApplicationException
    {
        public string[] ValidationErrors { get; }

        public ValidationException(string message) : base(message)
        {
            ValidationErrors = new[] { message };
        }

        public ValidationException(string[] errors) : base(string.Join("; ", errors))
        {
            ValidationErrors = errors;
        }
    }

    /// <summary>
    /// Exception thrown when a requested entity is not found
    /// </summary>
    public class EntityNotFoundException : ApplicationException
    {
        public string EntityType { get; }
        public object EntityId { get; }

        public EntityNotFoundException(string entityType, object entityId)
            : base($"{entityType} with ID '{entityId}' was not found")
        {
            EntityType = entityType;
            EntityId = entityId;
        }
    }

    /// <summary>
    /// Exception thrown when trying to create an entity that already exists
    /// </summary>
    public class EntityAlreadyExistsException : ApplicationException
    {
        public string EntityType { get; }
        public string ConflictProperty { get; }
        public object ConflictValue { get; }

        public EntityAlreadyExistsException(string entityType, string conflictProperty, object conflictValue)
            : base($"{entityType} with {conflictProperty} '{conflictValue}' already exists")
        {
            EntityType = entityType;
            ConflictProperty = conflictProperty;
            ConflictValue = conflictValue;
        }
    }

    /// <summary>
    /// Exception thrown when a business rule is violated
    /// </summary>
    public class BusinessRuleViolationException : ApplicationException
    {
        public string RuleName { get; }

        public BusinessRuleViolationException(string ruleName, string message)
            : base($"Business rule violation ({ruleName}): {message}")
        {
            RuleName = ruleName;
        }
    }

    /// <summary>
    /// Exception thrown when external service operations fail
    /// </summary>
    public class ExternalServiceException : ApplicationException
    {
        public string ServiceName { get; }

        public ExternalServiceException(string serviceName, string message)
            : base($"External service '{serviceName}' error: {message}")
        {
            ServiceName = serviceName;
        }

        public ExternalServiceException(string serviceName, string message, Exception innerException)
            : base($"External service '{serviceName}' error: {message}", innerException)
        {
            ServiceName = serviceName;
        }
    }

    /// <summary>
    /// Exception thrown when database operations fail
    /// </summary>
    public class DataAccessException : ApplicationException
    {
        public string Operation { get; }

        public DataAccessException(string operation, string message)
            : base($"Data access error during {operation}: {message}")
        {
            Operation = operation;
        }

        public DataAccessException(string operation, string message, Exception innerException)
            : base($"Data access error during {operation}: {message}", innerException)
        {
            Operation = operation;
        }
    }
} 