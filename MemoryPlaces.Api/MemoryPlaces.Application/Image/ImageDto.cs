namespace MemoryPlaces.Application.Image;

public class ImageDto
{
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public long Size { get; set; }
    public byte[] Content { get; set; } = default!;
    public string? BlobUri { get; set; }
}
