using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _tasksService;
        private readonly IUserService _userService;

        public TasksController(ITasksService tasksService, IUserService userService)
        {
            _tasksService = tasksService;
            _userService = userService;
        }
        
        [HttpPost("create/{userId:Guid}")]
        public async Task<IActionResult> CreateTaskAsync(Guid userId, CreateTaskDto body)
        {
            UserTask task = new UserTask
            {
                UserId = userId,
                Title = body.Title,
                Description = body.Description,
                DueDate = body.DueDate,
                TaskPriorizationId = body.Priority
            };

            await _tasksService.CreateUserTaskAsync(task);
            
            return StatusCode(StatusCodes.Status201Created, new
            {
                task = body
            });    
        }

        [HttpGet("get/{userId:Guid}")]
        public async Task<IActionResult> GetAllTasksAsync(Guid userId)
        {
            var checkUser = await _userService.GetUserByIdAsync(userId);

            if (checkUser is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    detail = "User not found"
                });
            }

            var tasksList = await _tasksService.GetAllUserTaskAsync(userId);

            return StatusCode(StatusCodes.Status200OK, tasksList);
        }
    }
}
