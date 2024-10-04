namespace Task.API.Models.Dtos;

public class TaskItemDto
{
    public Guid Id { get; set; } = new Guid();
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public List<string> FilePaths { get; set; } = new List<string>();
        
    public Guid AssignerId { get; set; }
    public List<Guid> Assignees { get; set; }
}
