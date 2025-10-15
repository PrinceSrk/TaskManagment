using TaskManagment.Dto.RequestDto;
using TaskManagment.Dto.ResponseDto;

namespace TaskManagment.Interfaces;

public interface IAuthService
{
    Task<APIResponse<int>> UserRegistration(UserRegisterDto dto);
    Task<APIResponse<LoginResponse>> UserLogin(LoginDto loginDto);
}
