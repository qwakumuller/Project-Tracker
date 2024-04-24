namespace ProjectTracker.Models;

public class Status
{
    public int StatusId { get; set; }
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }
    public int ProjectId { get; set; }
}
