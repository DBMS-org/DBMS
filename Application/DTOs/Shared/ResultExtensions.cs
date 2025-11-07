namespace Application.DTOs.Shared;

// Extension methods for Result types
public static class ResultExtensions
{
    // Transforms the value if result is successful
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> func)
    {
        return result.IsSuccess 
            ? Result.Success(func(result.Value)) 
            : Result.Failure<TOut>(result.Errors.Length > 0 ? result.Errors : new[] { result.Error });
    }

    // Chains Result-returning functions
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> func)
    {
        return result.IsSuccess 
            ? func(result.Value) 
            : Result.Failure<TOut>(result.Errors.Length > 0 ? result.Errors : new[] { result.Error });
    }

    // Executes action if successful
    public static Result<T> Tap<T>(this Result<T> result, Action<T> action)
    {
        if (result.IsSuccess)
            action(result.Value);
        
        return result;
    }

    // Executes action if failed
    public static Result<T> OnFailure<T>(this Result<T> result, Action<string> action)
    {
        if (result.IsFailure)
            action(result.Error);

        return result;
    }

    // Executes action if failed with all errors
    public static Result<T> OnFailure<T>(this Result<T> result, Action<string[]> action)
    {
        if (result.IsFailure)
            action(result.Errors.Length > 0 ? result.Errors : new[] { result.Error });
        
        return result;
    }

    // Returns value or default if failed
    public static T ValueOrDefault<T>(this Result<T> result, T defaultValue = default!)
    {
        return result.IsSuccess ? result.Value : defaultValue;
    }

    // Converts to Result without value
    public static Result ToResult<T>(this Result<T> result)
    {
        return result.IsSuccess 
            ? Result.Success() 
            : Result.Failure(result.Errors.Length > 0 ? result.Errors : new[] { result.Error });
    }
} 