namespace MemoryPlaces.Domain.Entities;

public class Image
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string ImagePath { get; set; } = default!;

    public Guid PlaceId { get; set; }
    public Place Place { get; set; } = default!;
}
