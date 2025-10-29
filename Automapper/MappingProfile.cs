using AutoMapper;
using TaskManagment.Dto.RequestDto;
using TaskManagment.Dto.ResponseDto;
using TaskManagment.Models;

namespace TaskManagment.Automapper;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRegisterDto, User>()
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name ?? string.Empty))
        .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash ?? string.Empty))
        .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role ?? string.Empty));

        CreateMap<LoginDto, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty))
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash ?? string.Empty));

        CreateMap<UserImage, UserImageDto>()
            .ForMember(dest => dest.ImageData, opt => opt.MapFrom(src => Convert.ToBase64String(src.ImageData)))
            .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.ContentType));
            
        CreateMap<TaskRequestDto, Models.Task>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.title ?? string.Empty))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description ?? string.Empty))
            .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.assignTo ?? 0))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status ?? string.Empty))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.dueDate ?? DateOnly.MinValue));
    }
}
