using Microsoft.AspNetCore.Mvc;
using TaskManagment.Dto.RequestDto;
using TaskManagment.Dto.ResponseDto;
using TaskManagment.Interfaces;

namespace TaskManagment.Controllers;

[ApiController]
[Route(V)]
public class TaskController : ControllerBase
{
    private const string V = "api/[controller]";
    private const string V1 = "{TaskId}";
    private const string V2 = "status";
    private readonly ITaskService _taskService;
    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPut(V1)]
    public async Task<IActionResult> UpdateTask(int TaskId, [FromBody] TaskRequestDto taskDto)
    {
        ApiResponse<int> response = await _taskService.UpdateTask(TaskId, taskDto);
        return HandleResponse(response);
    }

    [HttpGet(V2)]
    public async Task<IActionResult> GetTasks([FromQuery] string? status)
    {
        ApiResponse<List<TaskResponse>> resoponse = await _taskService.GetTasks(status);
        return HandleResponse(resoponse);
    }

    public IActionResult HandleResponse<T>(ApiResponse<T> resoponse)
    {
        if (!resoponse.error_status)
        {
            return Ok(resoponse);
        }
        return NotFound(resoponse);
    }
}


