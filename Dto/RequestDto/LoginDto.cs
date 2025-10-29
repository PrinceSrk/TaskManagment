namespace TaskManagment.Dto.RequestDto;

public class LoginDto
{
    public string? Email { get; set; } 
    public string? PasswordHash { get; set; } 
}

public class UserRegisterDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? Role { get; set; }
}

public class UserImageUploadDto
{
    public byte[]? ImageData { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public int? UserId { get; set; }
}

public class TaskRequestDto
{
    public int? TaskId { get; set; }
    public string? title { get; set; }
    public string? description { get; set; }
    public int? assignTo { get; set; }
    public string? status { get; set; }
    public DateOnly? dueDate { get; set; }
}
