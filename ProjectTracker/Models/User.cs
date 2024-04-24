namespace ProjectTracker.Models;

public class User
{
    public int UserId { get; set; }
    public string LastName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public bool IsAdmin { get; set; }
    //public List<Project>? Projects { get; set; }
    //public List<Task>? Tasks { get; set; }
}