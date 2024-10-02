using MediatR;

namespace MemoryPlaces.Application.Place.Commands.Delete;

public class DeleteCommand : IRequest
{
    public string Id { get; set; } = default!;
}
