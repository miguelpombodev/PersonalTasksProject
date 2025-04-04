using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.Configuration;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Providers;
using PersonalTasksProject.Providers.Interfaces;

namespace PersonalTasksProject.Controllers
{
  [Route("api/tasks")]
  [ApiController]
  [Authorize]
  public class TasksController : ControllerBase
  {
    private readonly ITasksService _tasksService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IAppConfiguration _appConfiguration;

    public TasksController(ITasksService tasksService, IUserService userService, IMapper mapper, IAppConfiguration appConfiguration)
    {
      _tasksService = tasksService;
      _userService = userService;
      _mapper = mapper;
      _appConfiguration = appConfiguration ?? throw new ArgumentNullException(nameof(appConfiguration));
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTaskAsync(
        CreateTaskDto body,
        [FromServices] ITokenProvider tokenProvider,
        [FromServices] SmtpEmailProvider smtpEmailProvider,
        [FromHeader(Name = "Authorization")] string token)
    {
      var decodedToken = tokenProvider.DecodeToken(token);

      var userExisted = await _userService.GetUserByEmailAsync(decodedToken);

      body.UserId = userExisted.Result.Id;

      var task = _mapper.Map<CreateTaskDto, UserTask>(body);

      await _tasksService.CreateUserTaskAsync(task);

      var createdTaskEmailBody = _buildCreatedTaskEmailBody(body);

      await smtpEmailProvider.SendEmail(userExisted.Result.Email, "A task have been created!", createdTaskEmailBody);

      return StatusCode(StatusCodes.Status201Created, body);
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetAllTasksAsync(
        [FromServices] ITokenProvider tokenProvider,
        [FromHeader(Name = "Authorization")] string token,
      [FromQuery] PaginationParameters pagination
      )
    {
      var decodedToken = tokenProvider.DecodeToken(token);

      var checkUserResult = await _userService.GetUserByEmailAsync(decodedToken);

      if (!checkUserResult.IsSuccess)
      {
        return StatusCode(StatusCodes.Status404NotFound, new
        {
          detail = checkUserResult.ErrorMessage
        });
      }

      var tasksList = await _tasksService.GetAllUserTaskAsync(checkUserResult.Result.Id, pagination);
      
      Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(tasksList.Result.Pagination));

      return StatusCode(StatusCodes.Status200OK, tasksList.Result.Data);
    }

    [HttpDelete("delete/{taskId:Guid}")]
    public async Task<IActionResult> DeleteTaskAsync(Guid taskId)
    {
      var checkTaskUserResult = await _tasksService.GetUserTaskByIdAsync(taskId);

      if (!checkTaskUserResult.IsSuccess)
      {
        return StatusCode(StatusCodes.Status404NotFound, new
        {
          detail = checkTaskUserResult.ErrorMessage
        });
      }

      await _tasksService.DeleteUserTaskAsync(taskId);

      return StatusCode(StatusCodes.Status200OK, new
      {
        detail = "Task successfully deleted!"
      });
    }

    [HttpGet("get/tasks-priorities")]
    public async Task<IActionResult> GetTasksPrioritiesAsync()
    {
      var taskPriorities = await _tasksService.GetTasksPrioritiesAsync();

      return StatusCode(StatusCodes.Status200OK, taskPriorities.Result);
    }

    private string _buildCreatedTaskEmailBody(CreateTaskDto task)
    {
      var rawEmailBody = _appConfiguration.EmailBodies["Create Task Email"].Replace("{task_title}", task.Title).Replace("{task_description}", task.Description).Replace("{task_due_date}", task.DueDate.ToString("dd/MM/yyyy"));
      return rawEmailBody;
    }
  }
}
