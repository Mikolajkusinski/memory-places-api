namespace MemoryPlaces.Application.Image;

public class ImageDataDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string ImagePath { get; set; } = default!;

    public string PlaceId { get; set; } = default!;
}
