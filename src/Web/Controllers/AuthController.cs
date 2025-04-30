using Application.Features.Authentication.Abstractions;
using Application.Features.Authentication.Login;
using Application.Features.Authentication.Register;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request, CancellationToken ct)
    {
        var response = await authService.RegisterAsync(request, ct);
        return Created("/api/users/{user.Username}", response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken ct)
    {
        var response = await authService.LoginAsync(request, ct);
        return Ok(response);
    }

    [HttpDelete("logout")]
    public ActionResult<string> Logout()
    {
        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<string>> Refresh(CancellationToken ct)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refresh_token", out var refreshToken))
            return Unauthorized("Refresh token not found.");

        var response = await authService.RefreshTokenAsync(refreshToken, ct);
        return Ok(response);
    }
}