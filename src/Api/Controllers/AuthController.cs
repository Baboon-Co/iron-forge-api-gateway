using System.Net;
using Api.Extensions;
using Application.Features.Authentication.Abstractions;
using Application.Features.Authentication.Login;
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

        if (authResult.IsSuccess)
            return StatusCode(StatusCodes.Status201Created, authResult.Value);

        var validationErrors = authResult.ToValidationErrorsDictionary();
        var requestError = authResult.GetRequestError();
        if (requestError.StatusCode == HttpStatusCode.Conflict)
            return Conflict(new ValidationProblemDetails(validationErrors));

        return BadRequest(new ValidationProblemDetails(validationErrors));
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
    public async Task<ActionResult<string>> Refresh(CancellationToken ct)
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refresh_token", out var refreshToken))
            return Unauthorized("Refresh token not found.");

        var response = await authService.RefreshTokenAsync(refreshToken, ct);
        return Ok(response);
    }
}