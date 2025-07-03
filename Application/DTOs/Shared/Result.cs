namespace Application.DTOs.Shared;

/// <summary>
/// Represents the result of an operation without a return value
/// </summary>
public class Result
{
    public bool IsSuccess { get; protected set; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; protected set; } = string.Empty;
    public string[] Errors { get; protected set; } = Array.Empty<string>();

    protected Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    protected Result(bool isSuccess, string[] errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
        Error = errors.Length > 0 ? errors[0] : string.Empty;
    }

    /// <summary>
    /// Creates a successful result
    /// </summary>
    public static Result Success()
    {
        return new Result(true, string.Empty);
    }

    /// <summary>
    /// Creates a failed result with a single error message
    /// </summary>
    public static Result Failure(string error)
    {
        return new Result(false, error);
    }

    /// <summary>
    /// Creates a failed result with multiple error messages
    /// </summary>
    public static Result Failure(string[] errors)
    {
        return new Result(false, errors);
    }

    /// <summary>
    /// Creates a successful result with a value
    /// </summary>
    public static Result<T> Success<T>(T value)
    {
        return new Result<T>(value, true, string.Empty);
    }

    /// <summary>
    /// Creates a failed result with a value type
    /// </summary>
    public static Result<T> Failure<T>(string error)
    {
        return new Result<T>(default!, false, error);
    }

    /// <summary>
    /// Creates a failed result with a value type and multiple errors
    /// </summary>
    public static Result<T> Failure<T>(string[] errors)
    {
        return new Result<T>(default!, false, errors);
    }
}

/// <summary>
/// Represents the result of an operation with a return value
/// </summary>
/// <typeparam name="T">The type of the return value</typeparam>
public class Result<T> : Result
{
    public T Value { get; private set; }

    internal Result(T value, bool isSuccess, string error) : base(isSuccess, error)
    {
        Value = value;
    }

    internal Result(T value, bool isSuccess, string[] errors) : base(isSuccess, errors)
    {
        Value = value;
    }

    /// <summary>
    /// Implicit conversion from T to Result<T>
    /// </summary>
    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }
} 