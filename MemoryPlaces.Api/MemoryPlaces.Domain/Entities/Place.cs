using System;

namespace MemoryPlaces.Domain.Entities;

public class Place
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Latitude { get; set; } = default!;
    public decimal Longitude { get; set; } = default!;
    public string? WikipediaLink { get; set; }
    public string? WebsiteLink { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public bool IsVerified { get; set; } = default!;
    public DateTime? VerificationDate { get; set; }

    public Guid AuthorId { get; set; } = default!;
    public User Author { get; set; } = default!;

    public int TypeId { get; set; } = default!;
    public Type Type { get; set; } = default!;

    public int PeriodId { get; set; } = default!;
    public Period Period { get; set; } = default!;

    public int CategoryId { get; set; } = default!;
    public Category Category { get; set; } = default!;
}
