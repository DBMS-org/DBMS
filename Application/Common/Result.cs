namespace Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; protected set; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; protected set; } = string.Empty;

        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && !string.IsNullOrWhiteSpace(error))
                throw new InvalidOperationException("Successful result cannot have an error");

            if (!isSuccess && string.IsNullOrWhiteSpace(error))
                throw new InvalidOperationException("Failed result must have an error");

            IsSuccess = isSuccess;
            Error = error ?? string.Empty;
        }

        public static Result Success() => new Result(true, string.Empty);
        public static Result Failure(string error) => new Result(false, error);

        public static Result<T> Success<T>(T value) => new Result<T>(value, true, string.Empty);
        public static Result<T> Failure<T>(string error) => new Result<T>(default!, false, error);
    }

    public class Result<T> : Result
    {
        public T Value { get; private set; }

        protected internal Result(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            Value = value;
        }
    }
}
