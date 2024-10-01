namespace MemoryPlaces.Application.Place;

public class PlaceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? WikipediaLink { get; set; }
    public string? WebsiteLink { get; set; }
    public DateTime Created { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerificationDate { get; set; }

    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; } = default!;

    public int TypeId { get; set; }
    public string TypeName { get; set; } = default!;

    public int PeriodId { get; set; }
    public string PeriodName { get; set; } = default!;

    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = default!;
}
