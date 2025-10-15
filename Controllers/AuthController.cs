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
        APIResponse<int> resoponse = await _authService.UserRegistration(dto);
        return HandleResponse(resoponse);
    }

    [HttpPost("UserLogin")]
    public async Task<IActionResult> UserLogin([FromBody] LoginDto loginDto)
    {
        var resoponse = await _authService.UserLogin(loginDto);
        return HandleResponse(resoponse); 
    }

    public IActionResult HandleResponse<T>(APIResponse<T> resoponse)
    {
        if (!resoponse.error_status)
        {
            return Ok(resoponse);
        }
        return NotFound(resoponse);
    }

}
