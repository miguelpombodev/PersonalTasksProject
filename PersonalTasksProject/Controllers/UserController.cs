using Microsoft.AspNetCore.Mvc;
using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.DTOs.Responses;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Extensions;

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

    [HttpGet("get-user/{id:Guid}")]
    public async Task<IActionResult> GetOneUserAsync(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new
            {
                detail = "User not found"
            });
        }

        return StatusCode(StatusCodes.Status200OK, new
        {
            user = new CreatedUserCompletedResponseDto(user.Name, user.Email, user.AvatarUrl, user.Id)
        });
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUserAsync(
        CreateUserDto body
        )
    {
        try
        {
            User user = new User
            {
                Name = body.Name,
                Email = body.Email,
                Password = body.Password,
            };
            
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

    [HttpPut("update/{userId:Guid}")]
    public async Task<IActionResult> UpdateUserAsync(Guid userId, CreatedUserResponseDto body)
    {
        if (body.AreAllPropertiesNullables())
            return StatusCode(StatusCodes.Status400BadRequest, new
            {
                detail = "Please provide valid data! Check your form again!"
            });
        
        var user = await _userService.GetUserByIdAsync(userId);

        if (user is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new
            {
                detail = "User not found"
            });
        }

        _userService.UpdateUserAsync(user, body);

        return StatusCode(StatusCodes.Status200OK, new
        {
            detail = "success!"
        });
    }

    [HttpDelete("delete/{userId:Guid}")]
    public async Task<IActionResult> DeleteUserAsync(Guid userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);

        if (user is null)
        {
            return StatusCode(StatusCodes.Status404NotFound, new
            {
                detail = "User not found"
            });
        }

        await _userService.DeleteUserAsync(userId);

        return StatusCode(StatusCodes.Status200OK, new
        {
            detail = "success!"
        });
    }
}