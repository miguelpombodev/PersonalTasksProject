using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Providers;

namespace PersonalTasksProject.Controllers
{
  [Route("api/tasks")]
  [ApiController]
  [Authorize]
  public class TasksController : ControllerBase
  {
    private readonly IConfiguration _configuration;
    private readonly ITasksService _tasksService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public TasksController(ITasksService tasksService, IUserService userService, IMapper mapper, IConfiguration configuration)
    {
      _configuration = configuration;
      _tasksService = tasksService;
      _userService = userService;
      _mapper = mapper;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTaskAsync(
        CreateTaskDto body,
        [FromServices] TokenProvider tokenProvider,
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
        [FromServices] TokenProvider tokenProvider,
        [FromHeader(Name = "Authorization")] string token)
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

      var tasksList = await _tasksService.GetAllUserTaskAsync(checkUserResult.Result.Id);

      return StatusCode(StatusCodes.Status200OK, tasksList.Result);
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
      var rawEmailBody = _configuration.GetValue<string>("CreatedTaskEmailBody").Replace("{task_title}", task.Title).Replace("{task_description}", task.Description).Replace("{task_due_date}", task.DueDate.ToString("dd/MM/yyyy"));
      return rawEmailBody;
    }
  }
}
