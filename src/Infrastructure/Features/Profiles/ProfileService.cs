using Application.Features.Profiles.Abstractions;
using Application.Features.Profiles.CreateProfile;
using Application.Features.Profiles.GetProfile;
using FluentResults;
using Grpc.Core;
using Infrastructure.Extensions;
using Infrastructure.Grpc.Abstractions;
using IronForge.Contracts.ProfileService;
using Utility.Result.ResultErrors;
using Utility.Result.ResultErrors.Enums;

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

        var grpcError = rpcResponseResult.GetGrpcResultError();
        var errorReason = grpcError.StatusCode switch
        {
            StatusCode.NotFound => new RequestError("User not found.", RequestErrorType.NotFound),
            StatusCode.InvalidArgument => new RequestError("Validation errors.", RequestErrorType.BadRequest),
            _ => new RequestError("Unknown gRPC error.", RequestErrorType.Internal)
        };

        var validationErrors = rpcResponseResult.GetValidationErrors();
        return Result.Fail(errorReason).WithErrors(validationErrors);
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

        var grpcError = rpcResponseResult.GetGrpcResultError();
        var errorReason = grpcError.StatusCode switch
        {
            StatusCode.NotFound => new RequestError("User not found.", RequestErrorType.NotFound),
            StatusCode.InvalidArgument => new RequestError("Validation errors.", RequestErrorType.BadRequest),
            _ => new RequestError("Unknown gRPC error.", RequestErrorType.Internal)
        };

        var validationErrors = rpcResponseResult.GetValidationErrors();
        return Result.Fail(errorReason).WithErrors(validationErrors);
    }
}