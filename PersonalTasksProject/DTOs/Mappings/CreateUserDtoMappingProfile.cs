using AutoMapper;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.DTOs.Mappings;

public class CreateUserDtoMappingProfile : Profile
{
    public CreateUserDtoMappingProfile()
    {
        CreateMap<CreateUserDto, User>().ReverseMap();
    }
}