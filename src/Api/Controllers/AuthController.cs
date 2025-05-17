using Api.Extensions;
using Application.Features.Authentication.Abstractions;
using Application.Features.Authentication.Login;
using Application.Features.Authentication.RefreshTokens;
using Application.Features.Authentication.Register;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request, CancellationToken ct)
    {
        var authResult = await authService.RegisterAsync(request, ct);

        return authResult.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, authResult.Value)
            : authResult.ToErrorActionResult();
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken ct)
    {
        var authResult = await authService.LoginAsync(request, ct);

        if (authResult.IsSuccess)
            return Ok(authResult.Value);

        return Unauthorized("Wrong login or password.");
    }

    [HttpDelete("logout")]
    public ActionResult<string> Logout()
    {
        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<RefreshTokensResponse>> Refresh(CancellationToken ct)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refresh_token", out var refreshToken))
            return Unauthorized("Refresh token not found.");

        var responseResult = await authService.RefreshTokenAsync(refreshToken, ct);
        return responseResult.IsSuccess
            ? Ok(responseResult.Value)
            : responseResult.ToErrorActionResult();
    }
}