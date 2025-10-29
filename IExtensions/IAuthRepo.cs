using TaskManagment.Dto.RequestDto;
using TaskManagment.Dto.ResponseDto;
using TaskManagment.Models;

namespace TaskManagment.Extensions;

public interface IAuthRepo
{
    Task<int> AddNewUser(User newUser);
    Task<User> GetExistingUser(string Email);
    Task<LoginResponse> IsUserAutehnticate(User user);
    Task<LoginResponse> RevokeToken(RevokeToken revokeToken);
    Task<bool> DeleteToken(RevokeToken revokeToken);
    Task<List<User>> GetActiveUsers();
    Task<bool> UploadImage(int userId, UserImageUploadDto userImage);
    Task<List<UserImage>> GetUserImages(int userId);
}
