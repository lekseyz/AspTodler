using Application.User.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.Dtos;

namespace Presentation.Contorllers;

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
    ILogger<AuthController> _logger;
    IUserService _userService;
    
    public AuthController(ILogger<AuthController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<UserTokensResponse> Login([FromBody] AuthUserRequest authUser)
    {
        _logger.LogInformation($"Login {authUser}");
        var authResult = _userService.LoginUser(authUser.Email, authUser.Password).Result;
        return Ok(new UserTokensResponse(authResult.Token, authResult.RefreshToken));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<UserTokensResponse> Register([FromBody] AuthUserRequest authUser)
    {
        _logger.LogInformation($"Register {authUser}");
        var authResult = _userService.RegisterUser(authUser.Email, authUser.Password).Result;
        return CreatedAtRoute("GetUser",new {userId = authResult.User.Id}, new UserTokensResponse(authResult.Token, authResult.RefreshToken));
    }

    [HttpPost("get_token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<AccessTokenResponse> GetToken([FromBody] string refreshToken)
    {
        return Ok(new AccessTokenResponse(refreshToken));
        
        return Unauthorized(Problem(
            title: "Invalid refresh token",
            statusCode: StatusCodes.Status401Unauthorized,
            detail: "Refresh token expired"));
    }
}