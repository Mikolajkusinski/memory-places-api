using MediatR;

namespace MemoryPlaces.Application.Place.Commands.Update;

public class UpdateCommand : UpdatePlaceDto, IRequest<PlaceDto>
{
    public string GivenId { get; set; } = default!;
    public string? Locale { get; set; }
}
