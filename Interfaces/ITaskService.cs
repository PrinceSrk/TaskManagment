using TaskManagment.Dto.RequestDto;
using TaskManagment.Dto.ResponseDto;

namespace TaskManagment.Interfaces;

public interface ITaskService
{
     Task<APIResponse<int>> UpdateTask(int TaskId, TaskRequestDto taskDto);
    Task<APIResponse<List<TaskResponse>>> GetTasks(string OrderStatus);
    Task<APIResponse<List<TaskResponse>>> GetAllTasks();

}
