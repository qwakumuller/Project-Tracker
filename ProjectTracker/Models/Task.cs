namespace ProjectTracker.Models;

public class Task
{
    public int TaskId { get; set; }
    public int? ProjectId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsComplete { get; set; }
    public int? PrereqTaskId { get; set; } = default!;
    public int? UserId { get; set; }
    public int? StatusId { get; set; }
    public Status? Status { get; set; } = default!;
}