using AutoMapper;
using PersonalTasksProject.DTOs.Requests;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.DTOs.Mappings;

public class CreateTaskDtoMappingProfile : Profile
{
    public CreateTaskDtoMappingProfile()
    {
        CreateMap<CreateTaskDto, UserTask>().ReverseMap();
    }
}