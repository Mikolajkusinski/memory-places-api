using MediatR;

namespace MemoryPlaces.Application.Place.Commands.Create;

public class CreateCommand : CreatePlaceDto, IRequest<string> { }
