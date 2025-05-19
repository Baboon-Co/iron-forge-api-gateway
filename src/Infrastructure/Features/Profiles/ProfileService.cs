using Application.Features.Profiles.Abstractions;
using Application.Features.Profiles.CreateProfile;
using Application.Features.Profiles.GetProfile;
using BaboonCo.Utility.Result.Extensions;
using FluentResults;
using Infrastructure.Grpc.Abstractions;
using IronForge.Contracts.ProfileService;

namespace Infrastructure.Features.Profiles;

public class ProfileService(
    Profile.ProfileClient profileClient,
    IGrpcCallerService grpcCallerService
) : IProfileService
{
    public async Task<Result<GetUserProfileResponse>> GetUserProfileAsync(
        GetUserProfileRequest request,
        CancellationToken ct)
    {
        var rpcRequest = new GetProfileRequest
        {
            Nickname = request.Nickname,
        };

        var rpcResponseResult = await grpcCallerService.CallAsync(
            req => profileClient.GetProfileAsync(req, cancellationToken: ct).ResponseAsync,
            rpcRequest,
            "GetProfile");

        if (rpcResponseResult.IsSuccess)
        {
            var rpcResponse = rpcResponseResult.Value;
            return Result.Ok(new GetUserProfileResponse(
                rpcResponse.Wins,
                rpcResponse.Losses
            ));
        }

        return rpcResponseResult.GetRequestError();
    }

    public async Task<Result<CreateUserProfileResponse>> CreateUserProfileAsync(
        CreateUserProfileRequest request,
        CancellationToken ct)
    {
        var rpcRequest = new CreateProfileRequest
        {
            UserId = request.UserId,
            Nickname = request.Nickname,
        };

        var rpcResponseResult = await grpcCallerService.CallAsync(
            req => profileClient.CreateProfileAsync(
                req, cancellationToken: ct).ResponseAsync,
            rpcRequest,
            "GetProfile");

        if (rpcResponseResult.IsSuccess)
            return Result.Ok(new CreateUserProfileResponse());

        return rpcResponseResult.GetRequestError();
    }
}