namespace MemoryPlaces.Application.ApplicationUser;

public class CurrnetUser
{
    public CurrnetUser(string id, string email, string role)
    {
        Id = id;
        Email = email;
        Role = role;
    }

    public string Id { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }

    public bool IsInRole(string role) => Role.Contains(role);
}
