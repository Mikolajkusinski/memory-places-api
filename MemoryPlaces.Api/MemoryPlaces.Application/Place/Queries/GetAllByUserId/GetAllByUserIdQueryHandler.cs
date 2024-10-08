using AutoMapper;
using MediatR;
using MemoryPlaces.Application.Interfaces;
using MemoryPlaces.Domain.RepositoryInterfaces;

namespace MemoryPlaces.Application.Place.Queries.GetAllByUserId;

public class GetByUserIdQueryHandler : IRequestHandler<GetAllByUserIdQuery, IEnumerable<PlaceDto>>
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public GetByUserIdQueryHandler(
        IPlaceRepository placeRepository,
        IMapper mapper,
        IUserContext userContext
    )
    {
        _placeRepository = placeRepository;
        _mapper = mapper;
        _userContext = userContext;
    }

    public async Task<IEnumerable<PlaceDto>> Handle(
        GetAllByUserIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var places = await _placeRepository.GetAllByUserIdAsync(request.GivenUserId);
        var locale = request.Locale ?? "en";
        var placeDtos = _mapper.Map<IEnumerable<PlaceDto>>(
            places,
            opt => opt.Items["Locale"] = locale
        );

        return placeDtos;
    }
}
