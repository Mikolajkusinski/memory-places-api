using MediatR;

namespace MemoryPlaces.Application.Place.Queries.GetById;

public class GetByIdQuery : PlaceDto, IRequest<PlaceDto>
{
    public string GivenId { get; set; } = default!;
    public string? Locale { get; set; }
}
