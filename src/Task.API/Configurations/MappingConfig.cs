namespace Task.API.Configurations;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<TaskItem, TaskItemDto>().ReverseMap();
    }
}
