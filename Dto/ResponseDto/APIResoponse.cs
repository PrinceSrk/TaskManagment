namespace TaskManagment.Dto.ResponseDto;

public class ApiResponse<T>
{
    public bool error_status { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static ApiResponse<T> SuccesResponse(T data, string message)
    {
        return new ApiResponse<T>
        {
            error_status = false,
            Message = message,
            Data = data
        };
    }

     public static ApiResponse<T> FailureResponse( string message)
    {
        return new ApiResponse<T>
        {
            error_status = true,
            Message = message,
        };
    }
}
