using Application.Common.Abstractions;
using Application.Features.Authentication.Abstractions;
using FluentResults;
using IronForge.Contracts.AuthService;
using LoginRequest = Application.Features.Authentication.Login.LoginRequest;
using LoginResponse = Application.Features.Authentication.Login.LoginResponse;
using RefreshTokensResponse = Application.Features.Authentication.RefreshTokens.RefreshTokensResponse;
using RegisterRequest = Application.Features.Authentication.Register.RegisterRequest;
using RegisterResponse = Application.Features.Authentication.Register.RegisterResponse;

namespace Infrastructure.Features.Authentication;

public class AuthService(
    Auth.AuthClient authClient,
    IGrpcCallerService grpcCallerService
) : IAuthService
{
    public async Task<Result<RegisterResponse>> RegisterAsync(
        RegisterRequest request,
        CancellationToken ct)
    {
        var rpcRequest = new IronForge.Contracts.AuthService.RegisterRequest
        {
            Login = request.Login,
            Password = request.Password
        };

        var rpcResponseResult = await grpcCallerService.CallAsync(
            req => authClient.RegisterAsync(req, cancellationToken: ct).ResponseAsync,
            rpcRequest,
            "Register");

        if (rpcResponseResult.IsFailed)
            return rpcResponseResult.ToResult();

        var rpcResponse = rpcResponseResult.Value;
        return new RegisterResponse(
            Guid.Parse(rpcResponse.UserId),
            rpcResponse.AccessToken,
            rpcResponse.AccessTokenExpiration.ToDateTime(),
            rpcResponse.RefreshToken,
            rpcResponse.RefreshTokenExpiration.ToDateTime()
        );
    }

    public async Task<Result<LoginResponse>> LoginAsync(
        LoginRequest request,
        CancellationToken ct)
    {
        var rpcRequest = new IronForge.Contracts.AuthService.LoginRequest
        {
            Login = request.Login,
            Password = request.Password
        };
        var rpcResponseResult = await grpcCallerService.CallAsync(
            req => authClient.LoginAsync(req, cancellationToken: ct).ResponseAsync,
            rpcRequest,
            "Login");

        if (rpcResponseResult.IsFailed)
            return rpcResponseResult.ToResult();

        var rpcResponse = rpcResponseResult.Value;
        return new LoginResponse(
            Guid.Parse(rpcResponse.UserId),
            rpcResponse.AccessToken,
            rpcResponse.AccessTokenExpiration.ToDateTime(),
            rpcResponse.RefreshToken,
            rpcResponse.RefreshTokenExpiration.ToDateTime()
        );
    }

    public async Task<Result<RefreshTokensResponse>> RefreshTokenAsync(
        string refreshToken,
        CancellationToken ct)
    {
        var rpcRequest = new RefreshTokensRequest {RefreshToken = refreshToken};
        var rpcResponseResult = await grpcCallerService.CallAsync(
            req => authClient.RefreshTokensAsync(req, cancellationToken: ct).ResponseAsync,
            rpcRequest,
            "Refresh token");

        if (rpcResponseResult.IsFailed)
            return rpcResponseResult.ToResult();

        var rpcResponse = rpcResponseResult.Value;
        return new RefreshTokensResponse(
            rpcResponse.AccessToken,
            rpcResponse.AccessTokenExpiration.ToDateTime(),
            rpcResponse.RefreshToken,
            rpcResponse.RefreshTokenExpiration.ToDateTime()
        );
    }
}