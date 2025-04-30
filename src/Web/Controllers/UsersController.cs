using Application.Features.Users.GetProfile;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
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