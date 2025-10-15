using TaskManagment.Dto.RequestDto;
using TaskManagment.Dto.ResponseDto;
using TaskManagment.Extensions;
using TaskManagment.Interfaces;
using TaskManagment.Models;

namespace TaskManagment.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepo _authRepo;
    public AuthService(IAuthRepo authRepo)
    {
        _authRepo = authRepo;
    }

    public async Task<APIResponse<int>> UserRegistration(UserRegisterDto dto)
    {
        if (dto == null)
        {
            return APIResponse<int>.FailureResponse("User Registration data is null");
        }

        User newUser = new User
        {
            Name = dto.Name ?? string.Empty,
            PasswordHash = dto.PasswordHash ?? string.Empty,
            Email = dto.Email ?? string.Empty,
            Role = dto.Role ?? string.Empty
        };

        int userId = await _authRepo.AddNewUser(newUser);

        return APIResponse<int>.SuccesResponse(userId, "User Registration Succesfully");
    }

    public async Task<APIResponse<LoginResponse>> UserLogin(LoginDto loginDto)
    {
        if (loginDto == null)
        {
            return APIResponse<LoginResponse>.FailureResponse("login data is null");
        }

        User user = new User
        {
            Email = loginDto.Email ?? string.Empty,
            PasswordHash = loginDto.PasswordHash ?? string.Empty
        };

        LoginResponse response = await _authRepo.IsUserAutehnticate(user);

        if (response.Token == "Null")
        {
            return APIResponse<LoginResponse>.FailureResponse("Invalid Credentials!!");
        }

       return APIResponse<LoginResponse>.SuccesResponse(response, "User LoginSuccesfully");
    }
}

    