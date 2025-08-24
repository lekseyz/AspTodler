using Application.User.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Misc;
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
        
        if (authResult.IsFailure)
            return authResult.GetError.GetObjectResult();
        
        var auth = authResult.Value;
        
        return Ok(new UserTokensResponse(auth!.Token, auth.RefreshToken));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<UserTokensResponse> Register([FromBody] AuthUserRequest authUser)
    {
        _logger.LogInformation($"Register {authUser}");
        var authResult = _userService.RegisterUser(authUser.Email, authUser.Password).Result;
        
        if (authResult.IsFailure)
            return authResult.GetError.GetObjectResult();
        
        var auth = authResult.Value;
        
        return CreatedAtRoute("GetUser",new {userId = auth!.User.Id}, new UserTokensResponse(auth.Token, auth.RefreshToken));
    }

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserTokensResponse>> RefreshToken([FromQuery] Guid id, [FromBody] string refreshToken)
    {
        var authResult = await _userService.RefreshToken(id, refreshToken);
        if (authResult.IsFailure)
            return authResult.GetError.GetObjectResult();

        var auth = authResult.Value;
        return Ok(new UserTokensResponse(auth!.Token, auth.RefreshToken));
    }
}