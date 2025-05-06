using Application.Features.Authentication.Login;
using Application.Features.Authentication.RefreshTokens;
using Application.Features.Authentication.Register;
using FluentResults;

namespace Application.Features.Authentication.Abstractions;

public interface IAuthService
{
    Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<Result<RefreshTokensResponse>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}