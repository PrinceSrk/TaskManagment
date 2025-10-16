using AutoMapper;
using TaskManagment.Dto.RequestDto;
using TaskManagment.Dto.ResponseDto;
using TaskManagment.Extensions;
using TaskManagment.Interfaces;
using TaskManagment.Models;

namespace TaskManagment.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepo _authRepo;
    private readonly IMapper _mapper;
    public AuthService(IAuthRepo authRepo, IMapper mapper)
    {
        _authRepo = authRepo;
        _mapper = mapper;
    }

    public async Task<ApiResponse<int>> UserRegistration(UserRegisterDto dto)
    {
        if (dto == null)
        {
            return ApiResponse<int>.FailureResponse("User Registration data is null");
        }
        User newUser = _mapper.Map<User>(dto);

        // User newUser = new User
        // {
        //     Name = dto.Name ?? string.Empty,
        //     PasswordHash = dto.PasswordHash ?? string.Empty,
        //     Email = dto.Email ?? string.Empty,
        //     Role = dto.Role ?? string.Empty
        // };

        int userId = await _authRepo.AddNewUser(newUser);

        return ApiResponse<int>.SuccesResponse(userId, "User Registration Succesfully");
    }

    public async Task<ApiResponse<LoginResponse>> UserLogin(LoginDto loginDto)
    {
        if (loginDto == null)
        {
            return ApiResponse<LoginResponse>.FailureResponse("login data is null");
        }

        User user = _mapper.Map<User>(loginDto);

        // User user = new User
        // {
        //     Email = loginDto.Email ?? string.Empty,
        //     PasswordHash = loginDto.PasswordHash ?? string.Empty
        // };

        LoginResponse response = await _authRepo.IsUserAutehnticate(user);

        if (response.Token == "Null")
        {
            return ApiResponse<LoginResponse>.FailureResponse("Invalid Credentials!!");
        }

       return ApiResponse<LoginResponse>.SuccesResponse(response, "User LoginSuccesfully");
    }
}

