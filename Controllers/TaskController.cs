using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagment.Dto.RequestDto;
using TaskManagment.Dto.ResponseDto;
using TaskManagment.Interfaces;

namespace TaskManagment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{TaskId}")]
    public async Task<IActionResult> UpdateTask(int TaskId,[FromBody] TaskRequestDto taskDto)
    {
        ApiResponse<int> response = await _taskService.UpdateTask(TaskId, taskDto);
        return HandleResponse(response);
    }

    [Authorize]
    [HttpGet("status")]
    public async Task<IActionResult> GetTasks([FromQuery] string? status)
    {
        ApiResponse<List<TaskResponse>> response = await _taskService.GetTasks(status);
        return HandleResponse(response);
    }

    private IActionResult HandleResponse<T>(ApiResponse<T> response)
    {
        if (!response.error_status)
        {
            return Ok(response);
        }
        return NotFound(response);
    }
}
