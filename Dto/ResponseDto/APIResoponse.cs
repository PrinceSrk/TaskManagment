namespace TaskManagment.Dto.ResponseDto;

public class APIResponse<T>
{
    public bool error_status { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static APIResponse<T> SuccesResponse(T data, string message)
    {
        return new APIResponse<T>
        {
            error_status = false,
            Message = message,
            Data = data
        };
    }

     public static APIResponse<T> FailureResponse( string message)
    {
        return new APIResponse<T>
        {
            error_status = true,
            Message = message,
        };
    }
}
