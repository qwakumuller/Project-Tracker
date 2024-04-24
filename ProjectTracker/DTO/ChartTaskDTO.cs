namespace ProjectTracker.DTO;

public class ChartTaskDTO
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Name { get; set; } = default!;
    public int Id { get; set; }
    public string Type { get; set; } = "task";
    public double Progress { get; set; }
    public bool IsDisabled { get; set; }
    public List<int> Dependencies { get; set; } = new();
}
