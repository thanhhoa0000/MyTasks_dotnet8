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

    public static async Task<Results<Ok<IEnumerable<TaskItemDto>>, BadRequest<string>>> GetAllTasks(
        TaskContext context,
        IMapper mapper)
    {
        var tasks = await context.TaskItems.ToListAsync();

        return TypedResults.Ok(mapper.Map<IEnumerable<TaskItemDto>>(tasks));
    }

    public static async Task<Results<Ok<TaskItemDto>, NotFound<string>, BadRequest<string>>> GetTaskById(
        TaskContext context,
        IMapper mapper,
        Guid id)
    {
        try
        {
            if (!Guid.TryParse(id.ToString(), out _))
            {
                return TypedResults.BadRequest($"'{id}' is not a valid ID!");
            }

            var task = await context.TaskItems.SingleOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return TypedResults.NotFound($"Task with ID {id} not found!");
            }

            return TypedResults.Ok(mapper.Map<TaskItemDto>(task));
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    public static async Task<Results<Created, BadRequest<string>>> AddTask(
        TaskContext context,
        [AsParameters] IMapper mapper,
        TaskItemDto taskDto)
    {
        try
        {
            var task = mapper.Map<TaskItem>(taskDto);

            context.TaskItems.Add(task);
            await context.SaveChangesAsync();

            return TypedResults.Created($"/api/v1/tasks/{taskDto.Id}");
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    public static async Task<Results<Created, NotFound<string>, BadRequest<string>>> UpdateTask(
        TaskContext context,
        IMapper mapper,
        TaskItemDto taskDto)
    {
        try
        {
            var task = await context.TaskItems.SingleOrDefaultAsync(t => t.Id == taskDto.Id);

            if (task == null)
            {
                return TypedResults.NotFound($"Task with ID {taskDto.Id} not found!");
            }

            var taskEntry = context.Entry(task);
            taskEntry.CurrentValues.SetValues(taskDto);

            await context.SaveChangesAsync();

            return TypedResults.Created($"/api/v1/tasks/{taskDto.Id}");
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    public static async Task<Results<NoContent, NotFound, BadRequest<string>>> DeleteTask(
        TaskContext context,
        IMapper mapper,
        Guid id)
    {
        try
        {
            var task = await context.TaskItems.SingleOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return TypedResults.NotFound();
            }

            context.TaskItems.Remove(task);
            await context.SaveChangesAsync();

            return TypedResults.NoContent();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
}
