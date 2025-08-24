using Application.User.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Dtos;

namespace Presentation.Contorllers;

[ApiController]
[Route("/api/users")]
public class UserController : ControllerBase
{
    ILogger<UserController> _logger;
    IUserService _userService;
    UserRepository _userRepository;
    public UserController(ILogger<UserController> logger, IUserService userService, UserRepository userRepository)
    {
        _logger = logger;
        _userService = userService;
        _userRepository = userRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserResponse>>> GetUsers()
    {
        _logger.LogInformation("GetUsers");
        var users = await _userService.GetAll();
        List<UserResponse> userResponses = users.Select(u => new UserResponse(u.Id, u.Email)).ToList();
        return userResponses;
    }

    [HttpGet("{userId}", Name = "GetUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> GetUser([FromRoute] Guid userId)
    {
        var userResult = await _userService.Get(userId);

        if (userResult.IsFailure)
            return userResult.GetError.GetObjectResult();
        
        var user = userResult.Value;
        return Ok(user);
    }
}