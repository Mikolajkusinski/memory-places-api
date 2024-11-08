using AutoMapper;
using MediatR;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;

namespace MemoryPlaces.Application.Place.Queries.GetById;

public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, PlaceDto>
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IMapper _mapper;

    public GetByIdQueryHandler(IPlaceRepository placeRepository, IMapper mapper)
    {
        _placeRepository = placeRepository;
        _mapper = mapper;
    }

    public async Task<PlaceDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var place = await _placeRepository.GetByIdAsync(request.GivenId);

        if (place is null)
        {
            throw new NotFoundException("Place not found");
        }

        var locale = request.Locale ?? "en";
        var placeDto = _mapper.Map<PlaceDto>(place, opt => opt.Items["Locale"] = locale);

        return placeDto;
    }
}
