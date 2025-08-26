using Application.Common.ErrorTypes;
using Application.UserLogic.Interfaces;
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
        
        if (authResult.IsFailure)
        {
            var error = authResult.GetError;
            return error switch
            {
                AuthError authError => Unauthorized(authError.Message),
                { } baseError       => Problem(statusCode: StatusCodes.Status500InternalServerError, 
                                                title: "User login error", 
                                                detail: baseError.Message)
            };
        }
        
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
        {
            var error = authResult.GetError;
            return error switch
            {
                InputError inputError => Conflict(inputError.Message),
                { } baseError         => Problem(statusCode: StatusCodes.Status500InternalServerError, 
                                            title: "User login error",
                                            detail: baseError.Message)
            };
        }
        
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
        {
            var error = authResult.GetError;
            return error switch
            {
                AuthError authError         => Unauthorized(authError.Message),
                NotFoundError notFoundError => Unauthorized(notFoundError.Message),
                { } baseError               => Problem(statusCode: StatusCodes.Status500InternalServerError,
                                                    title: "Refresh token error",
                                                    detail: baseError.Message)
            };
        }
        
        var auth = authResult.Value;
        return Ok(new UserTokensResponse(auth!.Token, auth.RefreshToken));
    }
}