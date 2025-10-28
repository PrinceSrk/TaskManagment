using TaskManagment.Dto.RequestDto;
using TaskManagment.Dto.ResponseDto;
using TaskManagment.Models;

namespace TaskManagment.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<int>> UserRegistration(UserRegisterDto dto);
    Task<ApiResponse<LoginResponse>> UserLogin(LoginDto loginDto);
    Task<ApiResponse<string>> UserLogout(RevokeToken evokeToken);
    Task<List<User>> GetActiveUsers();
}
