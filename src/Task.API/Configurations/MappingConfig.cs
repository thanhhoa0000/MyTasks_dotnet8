namespace Task.API.Configurations;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<TaskItem, TaskItemDto>();
        });
        return mappingConfig;
    }
}
