namespace ProjectTracker.DTO;

public class UserDTO
{
    public int? Id { get; set; }
    public string LastName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string Email { get; set; } = default!;
}
