using TaskManagment.Dto.ResponseDto;

namespace TaskManagment.Extensions;

public interface ITaskRepo
{
    Task<int> AddNewTask(Models.Task newTask);
    Task<List<TaskResponseSp>> GetAllTaskByStatus(string? OrderStatus);
}
