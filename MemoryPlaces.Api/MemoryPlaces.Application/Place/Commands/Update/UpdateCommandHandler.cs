using AutoMapper;
using MediatR;
using MemoryPlaces.Application.Interfaces;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;

namespace MemoryPlaces.Application.Place.Commands.Update;

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, PlaceDto>
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public UpdateCommandHandler(
        IPlaceRepository placeRepository,
        IMapper mapper,
        IUserContext userContext
    )
    {
        _placeRepository = placeRepository;
        _mapper = mapper;
        _userContext = userContext;
    }

    public async Task<PlaceDto> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var place = await _placeRepository.GetByIdAsync(request.GivenId);
        var user = _userContext.GetCurrnetUser();

        if (place is null)
        {
            throw new NotFoundException("Place not found");
        }

        if (user == null)
        {
            throw new ForbidException();
        }

        var isAdmin = user!.IsInRole("Admin");
        var isAuthor = place.AuthorId.ToString() == user.Id;

        if (!isAdmin && !isAuthor)
        {
            throw new ForbidException();
        }

        place.Name = request.Name;
        place.Description = request.Description;
        place.Latitude = request.Latitude;
        place.Longitude = request.Longitude;
        place.WikipediaLink = request.WikipediaLink;
        place.WebsiteLink = request.WebsiteLink;
        place.TypeId = request.TypeId;
        place.PeriodId = request.PeriodId;
        place.CategoryId = request.CategoryId;
        place.IsVerified = false;
        place.VerificationDate = null;

        await _placeRepository.Commit();

        var locale = request.Locale ?? "en";
        var placeDto = _mapper.Map<PlaceDto>(place, opt => opt.Items["Locale"] = locale);

        return placeDto;
    }
}
