using MediatR;

namespace MemoryPlaces.Application.Image.Queries.GetImageById;

public class GetImageByIdQuery : IRequest<ImageDataDto>
{
    public int ImageId { get; set; }
}
