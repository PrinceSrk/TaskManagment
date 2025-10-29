using AutoMapper;
using TaskManagment.Dto.RequestDto;
using TaskManagment.Dto.ResponseDto;
using TaskManagment.Extensions;
using TaskManagment.Helper;
using TaskManagment.Interfaces;
using TaskManagment.Models;

namespace TaskManagment.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepo _authRepo;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    public AuthService(IAuthRepo authRepo, IMapper mapper , IEmailService emailService)
    {
        _authRepo = authRepo;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<ApiResponse<int>> UserRegistration(UserRegisterDto dto)
    {
        if (dto == null)
        {
            return ApiResponse<int>.FailureResponse("User Registration data is null");
        }
        User newUser = _mapper.Map<User>(dto);
        newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

        int userId = await _authRepo.AddNewUser(newUser);

        return ApiResponse<int>.SuccesResponse(userId, "User Registration Succesfully");
    }

    public async Task<ApiResponse<LoginResponse>> UserLogin(LoginDto loginDto)
    {
        if (loginDto == null)
        {
            return ApiResponse<LoginResponse>.FailureResponse("login data is null");
        }

        User existingUser = await _authRepo.GetExistingUser(loginDto.Email ?? string.Empty);

        if (existingUser == null)
        {
            return ApiResponse<LoginResponse>.FailureResponse("User doesn't exists");
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.PasswordHash, existingUser.PasswordHash);

        if(!isPasswordValid)
        {
            return ApiResponse<LoginResponse>.FailureResponse("Password is wrong");
        }

        User user = _mapper.Map<User>(loginDto);

        LoginResponse response = await _authRepo.IsUserAutehnticate(user);

        if (response.AccessToken == null)
        {
            return ApiResponse<LoginResponse>.FailureResponse("Invalid Credentials!!");
        }

        await _emailService.SendEmailAsync(loginDto.Email ?? string.Empty, "User Login", "User Login Sucessfully");

        return ApiResponse<LoginResponse>.SuccesResponse(response, "User LoginSuccesfully");
    }

    public async Task<ApiResponse<string>> UserLogout(RevokeToken revokeToken)
    {
        if (revokeToken == null)
        {
            return ApiResponse<string>.FailureResponse("Revoke token data is null");
        }

        await _authRepo.DeleteToken(revokeToken);

        return ApiResponse<string>.SuccesResponse(string.Empty, "User LogOut Successfully!!");
    }

    public async Task<List<User>> GetActiveUsers()
    {
        List<User> users = await _authRepo.GetActiveUsers();

        if (users == null)
        {
            return null;
        }
        return users;
    }

    public async Task<ApiResponse<List<ImageUploadResult>>> UploadImage(int userId, List<IFormFile> imageFiles)
    {
        const long MaxImageSizeBytes = 2 * 1024 * 1024;
        string[] allowedMimeType = ["image/jpeg", "image/jpg", "image/png"];
        string[] alllowedExtensions = [".jpg", ".jpeg", ".png"];

        if (imageFiles == null || imageFiles.Count == 0)
        {
            return ApiResponse<List<ImageUploadResult>>.FailureResponse("Image Data Is Null");
        }

        List<UserImage> existingImages = await _authRepo.GetUserImages(userId);

        HashSet<string> existingHashes = existingImages
                                .Select(img => ImageHepler.ComputeHash(img.ImageData))
                                .ToHashSet();

        List<ImageUploadResult> results = [];
        List<Task<ImageUploadResult>> uploadTask = [];

        foreach (var imageFile in imageFiles)
        {
            string extensions = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!alllowedExtensions.Contains(extensions))
            {
                results.Add(new ImageUploadResult
                {
                    FileName = imageFile.FileName,
                    Status = false,
                    Message = $"Invalid Extension {extensions}"
                });
                continue;
            }

            if (!allowedMimeType.Contains(imageFile.ContentType))
            {
                results.Add(new ImageUploadResult
                {
                    FileName = imageFile.FileName,
                    Status = false,
                    Message = $"{imageFile.ContentType} is not allowed "
                });
                continue;
            }

            if (imageFile.Length > MaxImageSizeBytes)
            {
                results.Add(new ImageUploadResult
                {
                    FileName = imageFile.FileName,
                    Status = false,
                    Message = $"{imageFile.FileName} is too large"
                });
                continue;
            }

            Byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }
            string hash = ImageHepler.ComputeHash(imageBytes);

            if (existingHashes.Contains(hash))
            {
                results.Add(new ImageUploadResult
                {
                    FileName = imageFile.FileName,
                    Status = false,
                    Message = "Image Already Exists"
                });
                continue;
            }

            UserImageUploadDto userImage = new UserImageUploadDto
            {
                UserId = userId,
                ImageData = imageBytes,
                FileName = imageFile.FileName,
                ContentType = imageFile.ContentType
            };
            uploadTask.Add(UploadSingleImage(userId, userImage));
            existingHashes.Add(hash);
        }

        ImageUploadResult[] uploadeResults = await System.Threading.Tasks.Task.WhenAll(uploadTask);
        results.AddRange(uploadeResults);

        return ApiResponse<List<ImageUploadResult>>.SuccesResponse(results, "Images Uploaded");
    }
    
    private async Task<ImageUploadResult> UploadSingleImage(int userId,UserImageUploadDto userImage)
    {
        bool success = await _authRepo.UploadImage(userId, userImage);

        return new ImageUploadResult
        {
            FileName = userImage.FileName,
            Status = success,
            Message = success ? null : "Upload Fails"
        };
    }
    
    public async Task<ApiResponse<List<UserImageDto>>> GetUserImages(int userId)
    {
        List<UserImage> userImages = await _authRepo.GetUserImages(userId);

        if (userImages == null)
        {
            return ApiResponse<List<UserImageDto>>.FailureResponse("Something went Wrong");
        }

        List<UserImageDto> images = _mapper.Map<List<UserImageDto>>(userImages);

        return ApiResponse<List<UserImageDto>>.SuccesResponse(images, "Image fetches Succesfully");
    }
}


