using AutoMapper;
using MediatR;
using MemoryPlaces.Domain.RepositoryInterfaces;

namespace MemoryPlaces.Application.Place.Commands.Create;

public class CreateCommandHandler : IRequestHandler<CreateCommand, string>
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IMapper _mapper;

    public CreateCommandHandler(IPlaceRepository placeRepository, IMapper mapper)
    {
        _placeRepository = placeRepository;
        _mapper = mapper;
    }

    public async Task<string> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        var place = _mapper.Map<Domain.Entities.Place>(request);
        await _placeRepository.CreateAsync(place);

        return place.Id.ToString();
    }
}
