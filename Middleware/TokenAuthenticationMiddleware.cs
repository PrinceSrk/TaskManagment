using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TaskManagment.Dto.ResponseDto;
using TaskManagment.Extensions;
using TaskManagment.Models;

namespace TaskManagment.Middleware;

public class TokenAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;
    public TokenAuthenticationMiddleware(RequestDelegate next,IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }
    
    public async System.Threading.Tasks.Task Invoke(HttpContext context ,TaskContext db)
    {
        var accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var refreshToken = context.Request.Headers["X-Refresh-Token"].FirstOrDefault();

        if(!string.IsNullOrEmpty(accessToken))
        {
            var UserAccesstoken = await db.UserTokens
                                  .Include(u => u.User)
                                  .FirstOrDefaultAsync(u => u.Token == accessToken && u.TokenType == "Access");

            if(UserAccesstoken != null)
            {
                if (UserAccesstoken.ExpiryTime > DateTime.Now)
                {
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email,UserAccesstoken.User.Email),
                        new Claim(ClaimTypes.Role,UserAccesstoken.User.Role)
                    };

                    var identity = new ClaimsIdentity(claims, "CustomToken");
                    context.User = new ClaimsPrincipal(identity);
                }
                else if (!string.IsNullOrEmpty(refreshToken))
                {
                    var UserRefreshToken = await db.UserTokens
                                                .FirstOrDefaultAsync(u => u.Token == refreshToken && u.TokenType == "Refresh");

                    if (refreshToken != null && UserRefreshToken?.ExpiryTime > DateTime.Now)
                    {
                        RevokeToken revokeToken = new RevokeToken
                        {
                            UserId = UserRefreshToken.UserId ?? 0,
                            AccessToken = UserAccesstoken.Token,
                            RefreshToken = UserRefreshToken.Token
                        };

                        using var scope = _scopeFactory.CreateScope();
                        var authRepo = scope.ServiceProvider.GetRequiredService<IAuthRepo>();

                        LoginResponse newToken = await authRepo.RevokeToken(revokeToken);

                        context.Response.Headers["Authorization"] = newToken.AccessToken;
                        context.Response.Headers["X-Refresh-Token"] = newToken.RefreshToken;

                        List<Claim> claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email,UserAccesstoken.User.Email),
                            new Claim(ClaimTypes.Role,UserAccesstoken.User.Role)
                        };

                        var identity = new ClaimsIdentity(claims, "CustomToken");
                        context.User = new ClaimsPrincipal(identity);
                    }
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Session Expired , Please Re-Login Again");
                    return;
                }
            }
        }
        await _next(context);
    }
}
