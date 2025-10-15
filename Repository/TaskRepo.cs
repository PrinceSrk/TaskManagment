using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using TaskManagment.Dto.ResponseDto;
using TaskManagment.Extensions;

namespace TaskManagment.Repository;

public class TaskRepo : ITaskRepo
{
    private readonly string _connectionString;
    public TaskRepo(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }
     public async Task<int> AddNewTask(Models.Task newTask)
    {
        try
        {
            using SqlConnection conn = new SqlConnection(_connectionString);

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@TaskId", newTask.TaskId);
            parameters.Add("@title", newTask.Title);
            parameters.Add("@description", newTask.Description);
            parameters.Add("@assignedTo", newTask.AssignedTo);
            parameters.Add("@status", newTask.Status);
            parameters.Add("@newTaskId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("AddOrUpdateTask",
                                    parameters,
                                    commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@newTaskId");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<TaskResponseSp>> GetAllTaskByStatus(string OrderStatus)
    {
        try
        {
            using SqlConnection conn = new SqlConnection(_connectionString);

            IEnumerable<TaskResponseSp> tasks =
                                await conn.QueryAsync<TaskResponseSp>("TaskDetails",
                                                                      new { @status = OrderStatus },
                                                                      commandType: CommandType.StoredProcedure);
            return tasks.ToList();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<List<TaskResponseSp>> GetAllTask()
    {
        try
        {
            using SqlConnection conn = new SqlConnection(_connectionString);

            IEnumerable<TaskResponseSp> tasks =
                    await conn.QueryAsync<TaskResponseSp>("GetAllTasks",
                                                           commandType: CommandType.StoredProcedure);
            return tasks.ToList();                                                                           
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
