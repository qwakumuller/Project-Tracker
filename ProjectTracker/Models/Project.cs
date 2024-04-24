namespace ProjectTracker.Models;

public class Project
{
    public int ProjectId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Status>? Statuses { get; set; } = new();
}