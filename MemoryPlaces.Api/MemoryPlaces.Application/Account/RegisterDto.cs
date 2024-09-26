namespace MemoryPlaces.Application.Account;

public class RegisterDto
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public bool IsActive { get; set; } = false;
    public bool IsConfirmed { get; set; } = false;
    public int RoleId { get; set; } = 1;
}
