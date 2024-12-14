namespace Common.DTOs;

public class Result<T>
{
    private Result(bool isSuccess, T? value = default, string? error = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(false, default, error);
    }
}