namespace Task.API.Models;

public class TaskItem
{
    [Key] 
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required] 
    public string Description { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public DateTime Deadline { get; set; }
    public bool Completed { get; set; } = false;
    public List<string> FilePaths { get; set; } = new List<string>();
    
    public Guid AssignerId { get; set; }
    public List<Guid> Assignees { get; set; }
}
