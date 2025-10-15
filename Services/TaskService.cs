using TaskManagment.Dto.RequestDto;
using TaskManagment.Dto.ResponseDto;
using TaskManagment.Extensions;
using TaskManagment.Interfaces;

namespace TaskManagment.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepo _taskRepo;
    public TaskService( ITaskRepo taskRepo)
    {
        _taskRepo = taskRepo;
    }

    public async Task<APIResponse<int>> UpdateTask(int TaskId, TaskRequestDto taskDto)
    {
        if (taskDto == null)
        {
            return APIResponse<int>.FailureResponse("taskdto is null");
        }

        Models.Task newTask = new Models.Task
        {
            TaskId = TaskId,
            Title = taskDto.title ?? string.Empty,
            Description = taskDto.description,
            AssignedTo = taskDto.assignTo,
            Status = taskDto.status ?? string.Empty,
        };

        int taskId = await _taskRepo.AddNewTask(newTask);
        return APIResponse<int>.SuccesResponse(taskId, "Task created or modifier succesfuly!!");
    }

    public async Task<APIResponse<List<TaskResponse>>> GetTasks(string OrderStatus)
    {
        List<TaskResponseSp> data = await _taskRepo.GetAllTaskByStatus(OrderStatus);

        if (!data.Any())
        {
            return APIResponse<List<TaskResponse>>.FailureResponse("Currently no tasks avilable");
        }

        List<TaskResponse> response = data.GroupBy(u => new { u.status }).Select(g => new TaskResponse
        {
            status = g.Key.status,
            taskData = g.Select(v => new TaskResponseSp
            {
                status = v.status,
                TaskId = v.TaskId,
                Title = v.Title,
                Name = v.Name
            }).ToList()
        }).ToList();

        return APIResponse<List<TaskResponse>>.SuccesResponse(response, "Recored List Succesfully");
    }

    public async Task<APIResponse<List<TaskResponse>>> GetAllTasks()
    {
        List<TaskResponseSp> data = await _taskRepo.GetAllTask();

        if (!data.Any())
        {
            return APIResponse<List<TaskResponse>>.FailureResponse("Currently no tasks avilable");
        }

        List<TaskResponse> response = data.GroupBy(u => new { u.status }).Select(g => new TaskResponse
        {
            status = g.Key.status,
            taskData = g.Select(v => new TaskResponseSp
            {
                status = v.status,
                TaskId = v.TaskId,
                Title = v.Title,
                Name = v.Name
            }).ToList()
        }).ToList();

        return APIResponse<List<TaskResponse>>.SuccesResponse(response,"Recored List Succesfully");
    }
}


