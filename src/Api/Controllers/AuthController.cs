using Application.Errors;
using Application.Features.Authentication.Abstractions;
using Application.Features.Authentication.Login;
using Application.Features.Authentication.Register;
using Infrastructure.Grpc;
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

        var validationErrors = authResult.Errors
            .OfType<ValidationError>()
            .GroupBy(e => e.Field)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.Message).ToArray()
            );

        var grpcError = authResult.Errors.OfType<GrpcResultError>().First();
        if (grpcError.StatusCode == Grpc.Core.StatusCode.AlreadyExists)
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