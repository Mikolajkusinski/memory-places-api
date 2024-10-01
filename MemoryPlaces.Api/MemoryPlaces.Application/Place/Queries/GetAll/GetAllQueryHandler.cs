using AutoMapper;
using MediatR;
using MemoryPlaces.Domain.RepositoryInterfaces;

namespace MemoryPlaces.Application.Place.Queries.GetAll;

public class GetAllQueryHandler : IRequestHandler<GetAllQuery, IEnumerable<PlaceDto>>
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IMapper _mapper;

    public GetAllQueryHandler(IPlaceRepository placeRepository, IMapper mapper)
    {
        _placeRepository = placeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PlaceDto>> Handle(
        GetAllQuery request,
        CancellationToken cancellationToken
    )
    {
        var places = await _placeRepository.GetAllAsync();
        var locale = request.Locale ?? "en";
        var placeDtos = _mapper.Map<IEnumerable<PlaceDto>>(
            places,
            opt => opt.Items["Locale"] = locale
        );

        return placeDtos;
    }
}
