namespace SafaFoods.Core.Services.Models;

public sealed record ServiceResult<T>(
    bool Success,
    T? Data,
    string? ErrorCode,
    string? ErrorMessage)
{
    public static ServiceResult<T> Ok(T data) => new(true, data, null, null);

    public static ServiceResult<T> Fail(string errorCode, string errorMessage) =>
        new(false, default, errorCode, errorMessage);
}
