using TaskManagment.Dto.RequestDto;
using TaskManagment.Dto.ResponseDto;
using TaskManagment.Models;

namespace TaskManagment.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<int>> UserRegistration(UserRegisterDto dto);
    Task<ApiResponse<LoginResponse>> UserLogin(LoginDto loginDto);
    Task<ApiResponse<string>> UserLogout(RevokeToken evokeToken);
    Task<ApiResponse<List<ImageUploadResult>>> UploadImage(int userId, List<IFormFile> imageFile);
    Task<ApiResponse<List<UserImageDto>>> GetUserImages(int userId);
    Task<List<User>> GetActiveUsers();
}
