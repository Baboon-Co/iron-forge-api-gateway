using Application.Features.Users.GetProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("/api/users")]
public class UsersController : ControllerBase
{
    [HttpGet("{username}")]
    public async Task<GetUserProfileResponse> GetUserProfile([FromRoute] GetUserProfileRequest request)
    {
        return new GetUserProfileResponse();
    }
    
    [HttpGet]
    public string GetAllUsers()
    {
        throw new NotImplementedException();
    }
}