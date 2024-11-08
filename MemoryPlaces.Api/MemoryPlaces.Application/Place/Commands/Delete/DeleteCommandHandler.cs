using MediatR;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;

namespace MemoryPlaces.Application.Place.Commands.Delete;

public class DeleteCommandHandler : IRequestHandler<DeleteCommand>
{
    private readonly IPlaceRepository _placeRepository;

    public DeleteCommandHandler(IPlaceRepository placeRepository)
    {
        _placeRepository = placeRepository;
    }

    public async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        var place = await _placeRepository.GetByIdAsync(request.Id);

        if (place is null)
        {
            throw new NotFoundException("Place not found");
        }

        _placeRepository.Remove(place);
        await _placeRepository.Commit();
    }
}
