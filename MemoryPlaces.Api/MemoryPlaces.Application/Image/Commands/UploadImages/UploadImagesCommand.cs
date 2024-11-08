using MediatR;

namespace MemoryPlaces.Application.Image.Commands.UploadImages;

public class UploadImagesCommand : IRequest<IEnumerable<string>>
{
    public List<ImageDto> Images { get; set; }
    public string PlaceId { get; set; }

    public UploadImagesCommand(List<ImageDto> images, string placeId)
    {
        Images = images;
        PlaceId = placeId;
    }
}
