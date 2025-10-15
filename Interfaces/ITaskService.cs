using TaskManagment.Dto.RequestDto;
using TaskManagment.Dto.ResponseDto;

namespace TaskManagment.Interfaces;

public interface ITaskService
{
     Task<ApiResponse<int>> UpdateTask(int TaskId, TaskRequestDto taskDto);
    Task<ApiResponse<List<TaskResponse>>> GetTasks(string? OrderStatus);

}
