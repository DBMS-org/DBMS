using FluentValidation;

namespace Application.Validators
{
    /// <summary>
    /// Base validator class that provides common validation rules and patterns
    /// </summary>
    /// <typeparam name="T">The type to validate</typeparam>
    public abstract class BaseValidator<T> : AbstractValidator<T>
    {
        // Base class for common functionality - no extension methods here
    }

    /// <summary>
    /// Extension methods for common validation rules
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Common validation rule for required strings with minimum length
        /// </summary>
        public static IRuleBuilderOptions<T, string> RequiredString<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            string fieldName,
            int minLength = 1,
            int maxLength = 255)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage($"{fieldName} is required")
                .Length(minLength, maxLength)
                .WithMessage($"{fieldName} must be between {minLength} and {maxLength} characters");
        }

        /// <summary>
        /// Common validation rule for email addresses
        /// </summary>
        public static IRuleBuilderOptions<T, string> ValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid email format")
                .MaximumLength(255)
                .WithMessage("Email must not exceed 255 characters");
        }

        /// <summary>
        /// Common validation rule for positive integers
        /// </summary>
        public static IRuleBuilderOptions<T, int> PositiveInteger<T>(this IRuleBuilder<T, int> ruleBuilder, string fieldName)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage($"{fieldName} must be greater than 0");
        }

        /// <summary>
        /// Common validation rule for non-negative doubles
        /// </summary>
        public static IRuleBuilderOptions<T, double> NonNegativeDouble<T>(this IRuleBuilder<T, double> ruleBuilder, string fieldName)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(0)
                .WithMessage($"{fieldName} must be greater than or equal to 0");
        }

        /// <summary>
        /// Common validation rule for coordinate values
        /// </summary>
        public static IRuleBuilderOptions<T, double> ValidCoordinate<T>(this IRuleBuilder<T, double> ruleBuilder, string fieldName)
        {
            return ruleBuilder
                .Must(value => !double.IsNaN(value) && !double.IsInfinity(value))
                .WithMessage($"{fieldName} must be a valid number");
        }

        /// <summary>
        /// Common validation rule for optional coordinate values
        /// </summary>
        public static IRuleBuilderOptions<T, double?> ValidOptionalCoordinate<T>(this IRuleBuilder<T, double?> ruleBuilder, string fieldName)
        {
            return ruleBuilder
                .Must(value => !value.HasValue || (!double.IsNaN(value.Value) && !double.IsInfinity(value.Value)))
                .WithMessage($"{fieldName} must be a valid number when provided");
        }

        /// <summary>
        /// Common validation rule for password strength
        /// </summary>
        public static IRuleBuilderOptions<T, string> StrongPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long")
                .Matches(@"[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]")
                .WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"\d")
                .WithMessage("Password must contain at least one digit")
                .Matches(@"[^\w\s]")
                .WithMessage("Password must contain at least one special character");
        }
    }
} 