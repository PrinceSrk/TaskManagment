using TaskManagment.Dto.ResponseDto;
using TaskManagment.Models;

namespace TaskManagment.Extensions;

public interface IAuthRepo
{
    Task<int> AddNewUser(User newUser);
    Task<LoginResponse> IsUserAutehnticate(User user);
}
