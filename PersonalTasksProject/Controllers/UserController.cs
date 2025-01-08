using Microsoft.AspNetCore.Mvc;
using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUserAsync(
        User user
        )
    {
        try
        {
            await _userService.CreateUserAsync(user);
            return StatusCode(StatusCodes.Status201Created, new
            {
                detail = "success!"
            });
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}