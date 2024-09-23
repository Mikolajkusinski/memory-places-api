using System;
using System.Collections.Generic;

namespace MemoryPlaces.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public bool IsActive { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;

    public int RoleId { get; set; } = default!;
    public Role Role { get; set; } = default!;

    public List<Place> Places { get; set; } = new List<Place>();
}
