using MediatR;

namespace MemoryPlaces.Application.Place.Queries.GetAll;

public class GetAllQuery : IRequest<IEnumerable<PlaceDto>>
{
    public string? Locale { get; set; }
}
