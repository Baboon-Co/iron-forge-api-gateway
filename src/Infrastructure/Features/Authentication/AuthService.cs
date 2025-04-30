using Application.Abstractions;
using Application.Features.Authentication.Abstractions;
using IronForge.Contracts.AuthService;
using LoginRequest = Application.Features.Authentication.Login.LoginRequest;
using LoginResponse = Application.Features.Authentication.Login.LoginResponse;
using RefreshTokenResponse = Application.Features.Authentication.RefreshToken.RefreshTokenResponse;
using RegisterRequest = Application.Features.Authentication.Register.RegisterRequest;
using RegisterResponse = Application.Features.Authentication.Register.RegisterResponse;

namespace Infrastructure.Features.Authentication;

public class AuthService(
    Auth.AuthClient authClient,
    IGrpcCallerService grpcCallerService
) : IAuthService
{
    public async Task<RegisterResponse> RegisterAsync(
        RegisterRequest request,
        CancellationToken ct)
    {
        var rpcRequest = new IronForge.Contracts.AuthService.RegisterRequest
        {
            Username = request.Username,
            Login = request.Login,
            Password = request.Password
        };
        var rpcResponse = await grpcCallerService.CallAsync(
            req => authClient.RegisterAsync(req, cancellationToken: ct).ResponseAsync,
            rpcRequest,
            "Register");

        var userId = Guid.Parse(rpcResponse.UserId);
        return new RegisterResponse(userId, rpcResponse.AccessToken, rpcResponse.RefreshToken);
    }

    public async Task<LoginResponse> LoginAsync(
        LoginRequest request,
        CancellationToken ct)
    {
        var rpcRequest = new IronForge.Contracts.AuthService.LoginRequest
        {
            Login = request.Login,
            Password = request.Password
        };
        var rpcResponse = await grpcCallerService.CallAsync(
            req => authClient.LoginAsync(req, cancellationToken: ct).ResponseAsync,
            rpcRequest,
            "Login");

        return new LoginResponse(rpcResponse.AccessToken, rpcResponse.RefreshToken);
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(
        string refreshToken,
        CancellationToken ct)
    {
        var rpcRequest = new RefreshTokenRequest {RefreshToken = refreshToken};
        var rpcResponse = await grpcCallerService.CallAsync(
            req => authClient.RefreshTokenAsync(req, cancellationToken: ct).ResponseAsync,
            rpcRequest,
            "Refresh token");

        return new RefreshTokenResponse(rpcResponse.AccessToken, rpcResponse.RefreshToken);
    }
}