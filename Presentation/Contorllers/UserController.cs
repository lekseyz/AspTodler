using Application.Common.ErrorTypes;
using Application.UserLogic.Interfaces;
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
    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserResponse>>> GetUsers()
    {
        _logger.LogInformation("GetUsers");
        var users = await _userService.GetUsers();
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
        {
            var error = userResult.GetError;
            return error switch
            {
                NotFoundError notFoundError => NotFound(notFoundError.Message),
                {} baseError                => Problem(statusCode: StatusCodes.Status500InternalServerError,
                                                    title: "Get user error",
                                                    detail: baseError.Message)   
            };
        }
        
        var user = userResult.Value;
        return Ok(user);
    }
}