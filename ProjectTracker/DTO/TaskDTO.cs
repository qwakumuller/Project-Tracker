namespace ProjectTracker.DTO;

public class TaskDTO
{
    public int TaskId { get; set; }
    public int? ProjectId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsComplete { get; set; }
    public int? PrereqTaskId { get; set; }
    public int? UserId { get; set; }
    public string StatusName { get; set; } = default!;
    public UserDTO? User { get; set; } = default!;
    public PreTaskDTO? PreTask { get; set; } = default!;
}
