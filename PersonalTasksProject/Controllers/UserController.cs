using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.DTOs.Responses;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Extensions;
using PersonalTasksProject.Filters;
using PersonalTasksProject.Providers;
using PersonalTasksProject.Providers.Interfaces;

namespace PersonalTasksProject.Controllers;

[Route("api/users")]
[ApiController]
[ServiceFilter(typeof(LoggingFilter))]
public class UserController : ControllerBase
{
  private readonly int _maxFileSize = 30 * 1024 * 1024;

  private readonly IUserService _userService;
  private readonly IMapper _mapper;
  private readonly ITokenProvider _tokenProvider;

  public UserController(IUserService userService, IMapper mapper, ITokenProvider tokenProvider)
  {
    _userService = userService;
    _mapper = mapper;
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

    if (!DataEncryptionProvider.Verify(result.Result.Password, request.Password)) return StatusCode(StatusCodes.Status404NotFound, new
    {
      detail = "Check your email or password please"
    });

    var token = _tokenProvider.CreateToken(result.Result);

    return StatusCode(StatusCodes.Status200OK, new
    {
      token = token,
    });
  }

  [HttpGet("get")]
  public async Task<IActionResult> GetOneUserAsync([FromHeader(Name = "Authorization")] string token)
  {
    var decodedToken = _tokenProvider.DecodeToken(token);

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
    if (file.Length > _maxFileSize)
    {
      return StatusCode(StatusCodes.Status400BadRequest, new
      {
        detail = "File size is too big for the request."
      });
    }
    
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

