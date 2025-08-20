using AspTodler.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AspTodler.Controller;

[ApiController]
[Route("/api/users")]
public class UserController : ControllerBase
{
    ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<List<UserDto>> GetUsers()
    {
        _logger.LogInformation("GetUsers");
        return new List<UserDto>() { new UserDto("1488", "some@email.com"), new UserDto("1499", "some.other@email.com") };
    }

    [HttpGet("{userId}", Name = "GetUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<UserDto> GetUser([FromRoute] string userId)
    {
        _logger.LogInformation($"GetUser {userId}");
        return new UserDto(userId, "mail.mail.mail.mail.mail.mail.mail.mail.mail.mail.mail@mail.mail");
    }
}