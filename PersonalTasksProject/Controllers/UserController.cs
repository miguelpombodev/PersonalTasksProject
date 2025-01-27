using AutoMapper;
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
  private readonly IMapper _mapper;

  public UserController(IUserService userService, TokenProvider tokenProvider, IMapper mapper)
  {
    _userService = userService;
    _tokenProvider = tokenProvider;
    _mapper = mapper;
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

  [HttpGet("get")]
  public async Task<IActionResult> GetOneUserAsync([FromServices] TokenProvider tokenProvider, [FromHeader(Name = "Authorization")] string token)
  {
    var decodedToken = tokenProvider.DecodeToken(token);

    var result = await _userService.GetUserByEmailAsync(decodedToken);

    if (!result.IsSuccess) return StatusCode(StatusCodes.Status404NotFound, new
    {
      detail = result.ErrorMessage
    });

    return StatusCode(
        StatusCodes.Status200OK,
        new CreatedUserCompletedResponseDto(result.Result.Name, result.Result.Email, result.Result.AvatarUrl, result.Result.Id)
    );
  }

  [HttpPost("create")]
  public async Task<IActionResult> CreateUserAsync(
      [FromBody] CreateUserDto body
      )
  {
    try
    {
      var mappedUser = _mapper.Map<CreateUserDto, User>(body);

      await _userService.CreateUserAsync(mappedUser);
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

  [HttpPatch("update/avatar")]
  public async Task<IActionResult> UpdateAvatarImageAsync(
    [FromServices] TokenProvider tokenProvider,
    [FromServices] FileProvider fileProvider,
    [FromHeader(Name = "Authorization")] string token,
    IFormFile file
    )
  {
    var decodedToken = tokenProvider.DecodeToken(token);

    var userResult = await _userService.GetUserByEmailAsync(decodedToken);

    if (!userResult.IsSuccess) return StatusCode(StatusCodes.Status404NotFound, new
    {
      detail = userResult.ErrorMessage
    });
    
    var fileUrlPath = await fileProvider.SaveFileImageAsync(file);
    
    var resultFile = await _userService.UpdateUserAvatarAsync(userResult.Result.Id, fileUrlPath);

    if (!resultFile.IsSuccess)
    {
      return StatusCode(StatusCodes.Status400BadRequest, new
        {
          detail = resultFile.ErrorMessage
        }
      );
    }

    
    return StatusCode(StatusCodes.Status200OK, new
    {
      detail = "success!"
    });
  }
}

