using Microsoft.AspNetCore.Mvc;
using TaskManagment.Dto.RequestDto;
using TaskManagment.Dto.ResponseDto;
using TaskManagment.Interfaces;

namespace TaskManagment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("UserRegistration")]
    public async Task<IActionResult> UserRegistration([FromBody] UserRegisterDto dto)
    {
        ApiResponse<int> resoponse = await _authService.UserRegistration(dto);
        return HandleResponse(resoponse);
    }

    [HttpPost("UserLogin")]
    public async Task<IActionResult> UserLogin([FromBody] LoginDto loginDto)
    {
        ApiResponse<LoginResponse> resoponse = await _authService.UserLogin(loginDto);
        return HandleResponse(resoponse);
    }

    [HttpPost("UserLogOut")]
    public async Task<IActionResult> UserLogout([FromBody] RevokeToken revokeToken)
    {
        ApiResponse<string> response = await _authService.UserLogout(revokeToken);
        return HandleResponse(response);
    }

    [HttpPost("upload-user-image")]
    public async Task<IActionResult> UploadImage(int userId,List<IFormFile> imageFile)
    {
        ApiResponse<List<ImageUploadResult>> response = await _authService.UploadImage(userId, imageFile);
        return HandleResponse(response);
    }

    [HttpGet("user-images")]
    public async Task<IActionResult> GetUserImages(int userId)
    {
        ApiResponse<List<UserImageDto>> response = await _authService.GetUserImages(userId);
        return HandleResponse(response);
    }

    public IActionResult HandleResponse<T>(ApiResponse<T> resoponse)
    {
        if (!resoponse.error_status)
        {
            return Ok(resoponse);
        }
        return NotFound(resoponse);
    }

}


