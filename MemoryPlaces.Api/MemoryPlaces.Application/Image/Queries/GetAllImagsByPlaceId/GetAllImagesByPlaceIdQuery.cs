using MediatR;

namespace MemoryPlaces.Application.Image.Queries.GetAllImagesByPlaceId;

public class GetAllImagesByPlaceIdQuery : IRequest<IEnumerable<ImageDataDto>>
{
    public string PlaceId { get; set; } = default!;
}
