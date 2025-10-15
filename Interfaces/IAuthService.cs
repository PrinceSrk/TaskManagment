using TaskManagment.Dto.RequestDto;
using TaskManagment.Dto.ResponseDto;

namespace TaskManagment.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<int>> UserRegistration(UserRegisterDto dto);
    Task<ApiResponse<LoginResponse>> UserLogin(LoginDto loginDto);
}
