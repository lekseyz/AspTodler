using AspTodler.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AspTodler.Controller;

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
    ILogger<AuthController> _logger;
    
    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<UserTokenDto> Login([FromBody] AuthUserRequest authUser)
    {
        _logger.LogInformation($"Login {authUser}");
        return Ok(new UserTokenDto("token", "koken-huyoken"));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public ActionResult<UserTokenDto> Register([FromBody] AuthUserRequest authUser)
    {
        _logger.LogInformation($"Register {authUser}");
        var user = new UserDto("69", "mail@mail.mail");
        return CreatedAtRoute("GetUser",new {userId = user.Id}, new UserTokenDto("token", "koken-huyoken"));
    }
}