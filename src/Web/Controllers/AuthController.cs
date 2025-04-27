using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

internal record User(string Username, string Login, string Password);

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
    private static readonly List<User> _users = [];

    private AuthTokensResponseDto CreateTokens(User user)
    {
        return new AuthTokensResponseDto(
            $"{user.Username}+{user.Login.GetHashCode()}",
            $"{user.Username}+{user.Username.GetHashCode()}");
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthTokensResponseDto>> Register(RegisterRequestDto requestDto)
    {
        var user = new User(requestDto.Username, requestDto.Login, requestDto.Password);

        if (_users.Any(x => x.Login == user.Login))
            return Conflict($"User with login ${user.Login} already exists.");

        _users.Add(user);

        var response = CreateTokens(user);
        return Created("/api/users/{user.Username}", response);
    }

    [HttpPost("login")]
    public ActionResult<string> Login(LoginRequestDto requestDto)
    {
        var user = _users.FirstOrDefault(x => x.Login == requestDto.Login);
        if (user is null || user.Password != requestDto.Password)
            return Unauthorized("Login or password is incorrect.");

        var response = CreateTokens(user);
        return Ok(response);
    }

    [HttpDelete("logout")]
    public ActionResult<string> Logout()
    {
        return Ok();
    }

    [HttpPost("refresh")]
    public ActionResult<string> Refresh()
    {
        return Ok();
    }

    [HttpGet("test")]
    public ActionResult<string> Get()
    {
        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        var username = token.Split('+');
        if (username.Length < 2)
            return Forbid();

        var user = _users.FirstOrDefault(x => x.Username == username[0]);
        if (user is null)
            return Forbid();

        if (user.Login.GetHashCode().ToString() != username[1])
            return Forbid();

        return Ok("Success");
    }
}