using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using TaskManagment.Dto.ResponseDto;
using TaskManagment.Extensions;
using TaskManagment.Models;

namespace TaskManagment.Repository;

public class AuthRepo : IAuthRepo
{
    private readonly string _connectionString;
    public AuthRepo(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    public async Task<int> AddNewUser(User newUser)
    {
        try
        {
            using SqlConnection conn = new SqlConnection(_connectionString);

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Name", newUser.Name);
            parameters.Add("@PasswordHash", newUser.PasswordHash);
            parameters.Add("@Email", newUser.Email);
            parameters.Add("@Role", newUser.Role);
            parameters.Add("@userId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("UserRegistration",
                                    parameters,
                                    commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@userId");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<LoginResponse> IsUserAutehnticate(User user)
    {
        try
        {
            using SqlConnection conn = new SqlConnection(_connectionString);

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Email", user.Email);
            parameters.Add("@PasswordHash", user.PasswordHash);
            parameters.Add("@Role", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
            parameters.Add("@Token", dbType: DbType.String, size: 512, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("AuthenticateUser",
                                    parameters,
                                    commandType: CommandType.StoredProcedure);

            string UserRole = parameters.Get<string>("@Role");
            string UserToken = parameters.Get<string>("@Token");

            LoginResponse response = new LoginResponse
            {
                Role = UserRole,
                Token = UserToken
            };
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

