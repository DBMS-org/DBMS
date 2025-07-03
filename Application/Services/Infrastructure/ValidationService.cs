using Application.DTOs.Shared;
using Application.Exceptions;
using Application.Interfaces.Infrastructure;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Services.Infrastructure
{
    /// <summary>
    /// Service for validating DTOs using FluentValidation
    /// </summary>
    public class ValidationService : IValidationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ValidationService> _logger;

        public ValidationService(IServiceProvider serviceProvider, ILogger<ValidationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// Validates a DTO and returns validation result
        /// </summary>
        public async Task<Result> ValidateAsync<T>(T dto) where T : class
        {
            if (dto == null)
            {
                return Result.Failure("Input cannot be null");
            }

            try
            {
                var validator = _serviceProvider.GetService<IValidator<T>>();
                if (validator == null)
                {
                    _logger.LogWarning("No validator found for type {TypeName}", typeof(T).Name);
                    return Result.Success(); // No validation available, assume valid
                }

                var validationResult = await validator.ValidateAsync(dto);
                if (validationResult.IsValid)
                {
                    return Result.Success();
                }

                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
                _logger.LogWarning("Validation failed for {TypeName}: {Errors}", 
                    typeof(T).Name, string.Join(", ", errors));

                return Result.Failure(errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during validation of {TypeName}", typeof(T).Name);
                return Result.Failure("Validation error occurred");
            }
        }

        /// <summary>
        /// Validates a DTO and throws specific validation exception if validation fails
        /// </summary>
        public async Task ValidateAndThrowAsync<T>(T dto) where T : class
        {
            var result = await ValidateAsync(dto);
            if (result.IsFailure)
            {
                if (result.Errors.Length > 0)
                {
                    throw new ValidationException(result.Errors);
                }
                else
                {
                    throw new ValidationException(result.Error);
                }
            }
        }
    }
} 