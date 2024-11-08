using MediatR;
using MemoryPlaces.Application.Interfaces;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;

namespace MemoryPlaces.Application.Place.Commands.Verify;

public class VerifyCommandHandler : IRequestHandler<VerifyCommand>
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IUserContext _userContext;

    public VerifyCommandHandler(IPlaceRepository placeRepository, IUserContext userContext)
    {
        _placeRepository = placeRepository;
        _userContext = userContext;
    }

    public async Task Handle(VerifyCommand request, CancellationToken cancellationToken)
    {
        var place = await _placeRepository.GetByIdAsync(request.GivenId);
        var user = _userContext.GetCurrentUser();

        if (place is null)
        {
            throw new NotFoundException("Place not found");
        }

        if (user == null)
        {
            throw new ForbidException();
        }

        var isAdmin = user!.IsInRole("Admin");
        var isModerator = user!.IsInRole("Moderator");

        if (!isAdmin && !isModerator)
        {
            throw new ForbidException();
        }

        place.IsVerified = true;
        place.VerificationDate = DateTime.UtcNow;

        await _placeRepository.Commit();
    }
}
