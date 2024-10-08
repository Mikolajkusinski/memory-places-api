using MediatR;

namespace MemoryPlaces.Application.Place.Queries.GetAllByUserId;

public class GetAllByUserIdQuery : IRequest<IEnumerable<PlaceDto>>
{
    public string GivenUserId { get; set; } = default!;
    public string? Locale { get; set; }
}
