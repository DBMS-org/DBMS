using Application.DTOs.Shared;

namespace Application.Interfaces.Infrastructure
{
    /// <summary>
    /// Service for validating DTOs and requests before domain operations
    /// </summary>
    public interface IValidationService
    {
        /// <summary>
        /// Validates a DTO and returns validation result
        /// </summary>
        /// <typeparam name="T">Type of DTO to validate</typeparam>
        /// <param name="dto">DTO instance to validate</param>
        /// <returns>Result indicating success or validation errors</returns>
        Task<Result> ValidateAsync<T>(T dto) where T : class;

        /// <summary>
        /// Validates a DTO and throws exception if validation fails
        /// </summary>
        /// <typeparam name="T">Type of DTO to validate</typeparam>
        /// <param name="dto">DTO instance to validate</param>
        /// <returns>Task that completes when validation is successful</returns>
        /// <exception cref="ArgumentException">Thrown when validation fails</exception>
        Task ValidateAndThrowAsync<T>(T dto) where T : class;
    }
} 