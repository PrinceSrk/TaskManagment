namespace TaskManagment.Dto.ResponseDto;

public class LoginResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public string? Role { get; set; }
}

public class UserImageDto
{
    public byte[]? ImageData { get; set; }
    public string? ContentType { get; set; }
}

public class ImageUploadResult
{
    public string? FileName { get; set; }
    public bool Status { get; set; }
    public string? Message { get; set; }
}

public class RevokeToken
{
    public int UserId { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}

public class TaskResponse
{
    public string? status { get; set; }
    public List<TaskResponseSp>? taskData { get; set; }
}
public class TaskResponseSp
{
    public string? status { get; set; }
    public int TaskId { get; set; }
    public string? Title { get; set; }
    public string? Name { get; set; }
}