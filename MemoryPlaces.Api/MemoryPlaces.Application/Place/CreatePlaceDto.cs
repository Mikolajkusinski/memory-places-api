namespace MemoryPlaces.Application.Place;

public class CreatePlaceDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? WikipediaLink { get; set; }
    public string? WebsiteLink { get; set; }

    public Guid AuthorId { get; set; }

    public int TypeId { get; set; }
    public int PeriodId { get; set; }

    public int CategoryId { get; set; }
}
