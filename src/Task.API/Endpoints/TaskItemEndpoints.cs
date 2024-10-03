namespace Task.API.Endpoints;

public class TaskItemEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/tasks");

        group.MapGet("/", GetAllTasks);
    }

    public static IResult GetAllTasks()
    {
        return Results.Ok();
    }
}
