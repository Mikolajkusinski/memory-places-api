namespace MemoryPlaces.Application.Place;

public class CreatePlaceDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Latitude { get; set; } = default!;
    public decimal Longitude { get; set; } = default!;
    public string? WikipediaLink { get; set; }
    public string? WebsiteLink { get; set; }

    public Guid AuthorId { get; set; } = default!;

    public int TypeId { get; set; } = default!;

    public int PeriodId { get; set; } = default!;

    public int CategoryId { get; set; } = default!;
}
