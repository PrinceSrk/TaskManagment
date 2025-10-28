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
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    public AuthService(IAuthRepo authRepo, IMapper mapper , IEmailService emailService)
    {
        _authRepo = authRepo;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<ApiResponse<int>> UserRegistration(UserRegisterDto dto)
    {
        if (dto == null)
        {
            return ApiResponse<int>.FailureResponse("User Registration data is null");
        }
        User newUser = _mapper.Map<User>(dto);

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

        LoginResponse response = await _authRepo.IsUserAutehnticate(user);

        if (response.AccessToken == null)
        {
            return ApiResponse<LoginResponse>.FailureResponse("Invalid Credentials!!");
        }

        await _emailService.SendEmailAsync(loginDto.Email, "User Login", "User Login Sucessfully");

        return ApiResponse<LoginResponse>.SuccesResponse(response, "User LoginSuccesfully");
    }

    public async Task<ApiResponse<string>> UserLogout(RevokeToken revokeToken)
    {
        if (revokeToken == null)
        {
            return ApiResponse<string>.FailureResponse("Revoke token data is null");
        }

        bool success = await _authRepo.DeleteToken(revokeToken);

        return ApiResponse<string>.SuccesResponse(null, "User LogOut Successfully!!");
    }   
    
    public async Task<List<User>> GetActiveUsers()
    {
        List<User> users = await _authRepo.GetActiveUsers();

        if(users == null)
        {
            return null;
        }
        return users;
    }

}

