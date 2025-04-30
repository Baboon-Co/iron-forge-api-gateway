using Application.Features.Authentication.Login;
using Application.Features.Authentication.RefreshToken;
using Application.Features.Authentication.Register;

namespace Application.Features.Authentication.Abstractions;

public interface IAuthService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}