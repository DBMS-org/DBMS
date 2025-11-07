namespace Application.DTOs.Shared;

/// <summary>
/// Extension methods for Result types to provide fluent API
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Executes a function if the result is successful
    /// </summary>
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> func)
    {
        return result.IsSuccess 
            ? Result.Success(func(result.Value)) 
            : Result.Failure<TOut>(result.Errors.Length > 0 ? result.Errors : new[] { result.Error });
    }

    /// <summary>
    /// Executes a function that returns a Result if the current result is successful
    /// </summary>
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> func)
    {
        return result.IsSuccess 
            ? func(result.Value) 
            : Result.Failure<TOut>(result.Errors.Length > 0 ? result.Errors : new[] { result.Error });
    }

    /// <summary>
    /// Executes an action if the result is successful
    /// </summary>
    public static Result<T> Tap<T>(this Result<T> result, Action<T> action)
    {
        if (result.IsSuccess)
            action(result.Value);
        
        return result;
    }

    /// <summary>
    /// Executes an action if the result is a failure
    /// </summary>
    public static Result<T> OnFailure<T>(this Result<T> result, Action<string> action)
    {
        if (result.IsFailure)
            action(result.Error);
        
        return result;
    }

    /// <summary>
    /// Executes an action if the result is a failure with access to all errors
    /// </summary>
    public static Result<T> OnFailure<T>(this Result<T> result, Action<string[]> action)
    {
        if (result.IsFailure)
            action(result.Errors.Length > 0 ? result.Errors : new[] { result.Error });
        
        return result;
    }

    /// <summary>
    /// Returns the value if successful, otherwise returns the default value
    /// </summary>
    public static T ValueOrDefault<T>(this Result<T> result, T defaultValue = default!)
    {
        return result.IsSuccess ? result.Value : defaultValue;
    }

    /// <summary>
    /// Converts a Result<T> to a Result (discarding the value)
    /// </summary>
    public static Result ToResult<T>(this Result<T> result)
    {
        return result.IsSuccess 
            ? Result.Success() 
            : Result.Failure(result.Errors.Length > 0 ? result.Errors : new[] { result.Error });
    }
} 