namespace PersonalTasksProject.Helpers;

public class ServiceResult<T>
{
    public bool IsSuccess { get; }
    public T? Result { get; }
    public string? ErrorMessage { get; }

    private ServiceResult(bool isSuccess, T? result, string? errorMessage)
    {
        IsSuccess = isSuccess;
        Result = result;
        ErrorMessage = errorMessage;
    }

    public static ServiceResult<T> Success(T result) => new ServiceResult<T>(true, result, null);
    
    public static ServiceResult<T> Failure(string errorMessage) => new ServiceResult<T>(false, default, errorMessage);
}