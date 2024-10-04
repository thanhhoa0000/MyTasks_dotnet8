using Microsoft.AspNetCore.Http.HttpResults;

namespace Task.API.Endpoints;

public class TaskItemEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/tasks");

        group.MapGet("/", GetAllTasks);
        group.MapGet("/{id:guid}", GetAllTasks);
    }

    public static async Task<IResult> GetAllTasks(TaskContext context, IMapper mapper)
    {
        var tasks = await context.TaskItems.ToListAsync();

        return TypedResults.Ok(mapper.Map<IEnumerable<TaskItemDto>>(tasks));
    }

    public static async Task<Results<Ok<TaskItemDto>, BadRequest<string>>> GetTaskById(TaskContext context, IMapper mapper, Guid id)
    {
        try
        {
            var task = await context.TaskItems.FindAsync(id);

            return TypedResults.Ok(mapper.Map<TaskItemDto>(task));
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
}
