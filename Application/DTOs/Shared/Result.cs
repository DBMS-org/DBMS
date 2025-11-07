namespace Application.DTOs.Shared;

// Represents an operation result without a return value
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

    public static Result Success()
    {
        return new Result(true, string.Empty);
    }

    public static Result Failure(string error)
    {
        return new Result(false, error);
    }

    public static Result Failure(string[] errors)
    {
        return new Result(false, errors);
    }

    public static Result<T> Success<T>(T value)
    {
        return new Result<T>(value, true, string.Empty);
    }

    public static Result<T> Failure<T>(string error)
    {
        return new Result<T>(default!, false, error);
    }

    public static Result<T> Failure<T>(string[] errors)
    {
        return new Result<T>(default!, false, errors);
    }
}

// Represents an operation result with a return value
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

    // Implicit conversion from T to Result<T>
    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }
} 