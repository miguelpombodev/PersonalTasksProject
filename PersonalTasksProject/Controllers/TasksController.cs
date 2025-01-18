using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalTasksProject.Business.Interfaces;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    [Authorize]
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
            var task = new UserTask
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
            var checkUserResult = await _userService.GetUserByIdAsync(userId);

            if (!checkUserResult.IsSuccess)
            {
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    detail = checkUserResult.ErrorMessage
                });
            }

            var tasksList = await _tasksService.GetAllUserTaskAsync(userId);

            return StatusCode(StatusCodes.Status200OK, tasksList.Result);
        }

        [HttpDelete("delete/{taskId:Guid}")]
        public async Task<IActionResult> DeleteTaskAsync(Guid taskId)
        {
            var checkTaskUserResult = await _tasksService.GetUserTaskByIdAsync(taskId);

            if (checkTaskUserResult.IsSuccess)
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
    }
}
