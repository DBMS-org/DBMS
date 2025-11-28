using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Exceptions;

namespace API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorResponse();

            switch (exception)
            {
                case ValidationException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "Please correct the highlighted errors and try again.";
                    response.Errors = ex.ValidationErrors;
                    response.ErrorType = "VALIDATION_ERROR";
                    break;

                case EntityNotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = ex.Message;
                    response.ErrorType = "NOT_FOUND";
                    response.Details = new { EntityType = ex.EntityType, EntityId = ex.EntityId };
                    break;

                case EntityAlreadyExistsException ex:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.Message = ex.Message;
                    response.ErrorType = "ALREADY_EXISTS";
                    response.Details = new { EntityType = ex.EntityType, ConflictProperty = ex.ConflictProperty, ConflictValue = ex.ConflictValue };
                    break;

                case BusinessRuleViolationException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = ex.Message;
                    response.ErrorType = "BUSINESS_RULE_VIOLATION";
                    response.Details = new { RuleName = ex.RuleName };
                    break;

                case NotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = ex.Message;
                    response.ErrorType = "NOT_FOUND";
                    break;

                case UnauthorizedException ex:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = string.IsNullOrWhiteSpace(ex.Message)
                        ? "You are not authorized to access this resource. Please log in and try again."
                        : ex.Message;
                    response.ErrorType = "UNAUTHORIZED";
                    break;

                case ForbiddenException ex:
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    response.Message = string.IsNullOrWhiteSpace(ex.Message)
                        ? "You don't have permission to perform this action. Please contact your administrator if you believe this is an error."
                        : ex.Message;
                    response.ErrorType = "FORBIDDEN";
                    break;

                case ConflictException ex:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.Message = ex.Message;
                    response.ErrorType = "CONFLICT";
                    break;

                case ExternalServiceException ex:
                    response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    response.Message = "An external service is temporarily unavailable. Please try again in a few moments.";
                    response.ErrorType = "EXTERNAL_SERVICE_ERROR";
                    response.Details = new { ServiceName = ex.ServiceName, OriginalMessage = ex.Message };
                    _logger.LogError(exception, "External service error: {ServiceName}", ex.ServiceName);
                    break;

                case DataAccessException ex:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = "A database error occurred while processing your request. Please try again or contact support if the issue persists.";
                    response.ErrorType = "DATA_ACCESS_ERROR";
                    response.Details = new { Operation = ex.Operation };
                    _logger.LogError(exception, "Data access error during: {Operation}", ex.Operation);
                    break;

                case InternalServerException ex:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = string.IsNullOrWhiteSpace(ex.Message)
                        ? "An unexpected error occurred while processing your request. Our team has been notified."
                        : ex.Message;
                    response.ErrorType = "INTERNAL_ERROR";
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = "An unexpected error occurred. Our team has been notified and is working to resolve the issue.";
                    response.ErrorType = "UNKNOWN_ERROR";
                    _logger.LogError(exception, "Unhandled exception of type: {ExceptionType}", exception.GetType().Name);
                    break;
            }

            context.Response.StatusCode = response.StatusCode;
            _logger.LogError(exception, "API Error [{ErrorType}]: {Message}", response.ErrorType, exception.Message);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }

    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string[]? Errors { get; set; }
        public string? ErrorType { get; set; }
        public object? Details { get; set; }
    }
} 