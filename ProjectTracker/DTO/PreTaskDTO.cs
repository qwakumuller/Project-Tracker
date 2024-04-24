namespace ProjectTracker.DTO;

public class PreTaskDTO
{
    public int Id { get; set; }
    public string TaskName { get; set; } = default!;
    public bool IsComplete { get; set; }
}
