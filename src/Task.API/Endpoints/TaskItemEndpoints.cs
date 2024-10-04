using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Task.API.Endpoints;

public class TaskItemEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/tasks");

        group.MapGet("/", GetAllTasks);
        group.MapGet("/{id:guid}", GetAllTasks);

        group.MapPost("/", AddTask);
        group.MapPut("/", UpdateTask);

        group.MapDelete("/{id:guid}", DeleteTask);
    }

    public static async Task<IResult> GetAllTasks(TaskContext context, IMapper mapper)
    {
        var tasks = await context.TaskItems.ToListAsync();

        return TypedResults.Ok(mapper.Map<IEnumerable<TaskItemDto>>(tasks));
    }

    public static async Task<Results<Ok<TaskItemDto>, BadRequest<string>>> GetTaskById(
        TaskContext context,
        IMapper mapper,
        Guid id)
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

    public static async Task<Results<Ok<string>, BadRequest<string>>> AddTask(
        TaskContext context,
        [FromBody] IMapper mapper,
        TaskItemDto taskDto)
    {
        try
        {
            var task = mapper.Map<TaskItem>(taskDto);

            await context.TaskItems.AddAsync(task);
            await context.SaveChangesAsync();

            return TypedResults.Ok("Task added successfully!");
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    public static async Task<Results<Ok<string>, BadRequest<string>>> UpdateTask(
        TaskContext context,
        [FromBody] IMapper mapper,
        TaskItemDto taskDto)
    {
        try
        {
            var task = mapper.Map<TaskItem>(taskDto);

            context.TaskItems.Update(task);
            await context.SaveChangesAsync();

            return TypedResults.Ok("Task updated successfully!");
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    public static async Task<Results<Ok<string>, BadRequest<string>>> DeleteTask(
        TaskContext context,
        IMapper mapper,
        Guid id)
    {
        try
        {
            var task = await context.TaskItems.FindAsync(id);

            context.TaskItems.Remove(task);
            await context.SaveChangesAsync();

            return TypedResults.Ok("Task deleted successfully!");
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
}
