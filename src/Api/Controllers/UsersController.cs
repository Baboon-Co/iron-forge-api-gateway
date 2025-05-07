using Application.Features.Users.GetProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("/api/users")]
public class UsersController(ILogger<UsersController> logger) : ControllerBase
{
    [HttpGet("/profile/{username}")]
    public async Task<GetUserProfileResponse> GetUserProfile(string username)
    {
        var request = new GetUserProfileRequest(username);
        logger.LogDebug("Requesting profile for user: {Username}", request.Username);
        await Task.Delay(500);
        return new GetUserProfileResponse();
    }

    [HttpGet]
    public string GetAllUsers()
    {
        throw new NotImplementedException();
    }
}