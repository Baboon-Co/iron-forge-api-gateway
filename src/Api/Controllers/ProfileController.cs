using Api.Extensions;
using Application.Features.Profiles.Abstractions;
using Application.Features.Profiles.CreateProfile;
using Application.Features.Profiles.GetProfile;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/api/profiles")]
public class ProfileController(IProfileService profileService) : ControllerBase
{
    [HttpGet("{username}")]
    public async Task<ActionResult<GetUserProfileResponse>> GetUserProfile(
        [FromRoute] string username,
        CancellationToken ct)
    {
        var request = new GetUserProfileRequest(username);
        var responseResult = await profileService.GetUserProfileAsync(request, ct);
        return responseResult.IsSuccess
            ? Ok(responseResult.Value)
            : responseResult.ToErrorActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserProfileResponse>> CreateUserProfile(
        CreateUserProfileRequest request,
        CancellationToken ct)
    {
        var result = await profileService.CreateUserProfileAsync(request, ct);
        return result.IsSuccess
            ? Created($"/api/profiles/{request.Nickname}", result.Value)
            : result.ToErrorActionResult();
    }
}