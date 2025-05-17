using Application.Features.Profiles.CreateProfile;
using Application.Features.Profiles.GetProfile;
using FluentResults;

namespace Application.Features.Profiles.Abstractions;

public interface IProfileService
{
    Task<Result<GetUserProfileResponse>> GetUserProfileAsync(GetUserProfileRequest request, CancellationToken ct);
    Task<Result<CreateUserProfileResponse>> CreateUserProfileAsync(CreateUserProfileRequest request, CancellationToken ct);
}