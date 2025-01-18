using Microsoft.AspNetCore.Mvc;
using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.DTOs.Responses;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Extensions;
using PersonalTasksProject.Providers;

namespace PersonalTasksProject.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    
    private readonly IUserService _userService;
    private readonly TokenProvider _tokenProvider;

    public UserController(IUserService userService, TokenProvider tokenProvider)
    {
        _userService = userService;
        _tokenProvider = tokenProvider;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var result = await _userService.GetUserByEmailAsync(request.Email);
        
        if (!result.IsSuccess) return StatusCode(StatusCodes.Status404NotFound, new
        {
            detail = result.ErrorMessage
        });

        var token = _tokenProvider.CreateToken(result.Result);

        return StatusCode(StatusCodes.Status200OK, new
        {
            token = token,
        });
    }

    [HttpGet("get-user/{id:Guid}")]
    public async Task<IActionResult> GetOneUserAsync(Guid id)
    {
        var result = await _userService.GetUserByIdAsync(id);

        if (!result.IsSuccess) return StatusCode(StatusCodes.Status404NotFound, new
        {
            detail = result.ErrorMessage
        });

        return StatusCode(StatusCodes.Status200OK, new
        {
            user = new CreatedUserCompletedResponseDto(result.Result.Name, result.Result.Email, result.Result.AvatarUrl, result.Result.Id)
        });
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUserAsync(
        CreateUserDto body
        )
    {
        try
        {
            var user = new User
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
        
        var result = await _userService.GetUserByIdAsync(userId);

        if (!result.IsSuccess) return StatusCode(StatusCodes.Status404NotFound, new
        {
            detail = result.ErrorMessage
        });

        await _userService.UpdateUserAsync(result.Result, body);

        return StatusCode(StatusCodes.Status200OK, new
        {
            detail = "success!"
        });
    }

    [HttpDelete("delete/{userId:Guid}")]
    public async Task<IActionResult> DeleteUserAsync(Guid userId)
    {
        var resultUser = await _userService.GetUserByIdAsync(userId);

        if (!resultUser.IsSuccess) return StatusCode(StatusCodes.Status404NotFound, new
        {
            detail = resultUser.ErrorMessage
        });

        await _userService.DeleteUserAsync(userId);

        return StatusCode(StatusCodes.Status200OK, new
        {
            detail = "success!"
        });
    }
}