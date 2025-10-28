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
            parameters.Add("@AccessToken", dbType: DbType.String, size: 512, direction: ParameterDirection.Output);
            parameters.Add("@AccessToken", dbType: DbType.String, size: 512, direction: ParameterDirection.Output);
            parameters.Add("@RefreshToken", dbType: DbType.String, size: 512, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("AuthenticateUser",
                                    parameters,
                                    commandType: CommandType.StoredProcedure);

            string UserRole = parameters.Get<string>("@Role");
            string AccessToken = parameters.Get<string>("@AccessToken");
            string RefresToken = parameters.Get<string>("@RefreshToken");

            LoginResponse response = new LoginResponse
            {
                Role = UserRole,
                AccessToken = AccessToken,
                RefreshToken = RefresToken
            };
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<LoginResponse> RevokeToken(RevokeToken revokeToken)
    {
        try
        {
            using SqlConnection conn = new SqlConnection(_connectionString);

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@UserId", revokeToken.UserId);
            parameters.Add("@AccessToken", revokeToken.AccessToken);
            parameters.Add("@RefreshToken", revokeToken.RefreshToken);
            parameters.Add("@OutAccessToken", dbType: DbType.String, size: 512, direction: ParameterDirection.Output);
            parameters.Add("@OutRefreshToken", dbType: DbType.String, size: 512, direction: ParameterDirection.Output);

            await conn.ExecuteAsync("RevokeToken",
                                    parameters,
                                    commandType: CommandType.StoredProcedure);

            string AccessToken = parameters.Get<string>("@OutAccessToken");
            string RefreshToken = parameters.Get<string>("@OutRefreshToken");

            LoginResponse response = new LoginResponse
            {
                AccessToken = AccessToken,
                RefreshToken = RefreshToken
            };
            return response;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> DeleteToken(RevokeToken revokeToken)
    {
        try
        {
            using SqlConnection conn = new SqlConnection(_connectionString);

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@AccessToken", revokeToken.AccessToken);
            parameters.Add("@RefreshToken", revokeToken.RefreshToken);

            int rowAffected = await conn.ExecuteAsync("DeleteTokens",
                                    parameters,
                                    commandType: CommandType.StoredProcedure);

            return rowAffected > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<List<User>> GetActiveUsers()
    {
        try
        {
            using SqlConnection conn = new SqlConnection(_connectionString);

            IEnumerable<User> users = await conn.QueryAsync<User>("GetActiveUsers",
                                                                   commandType: CommandType.StoredProcedure);

            return users.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}

