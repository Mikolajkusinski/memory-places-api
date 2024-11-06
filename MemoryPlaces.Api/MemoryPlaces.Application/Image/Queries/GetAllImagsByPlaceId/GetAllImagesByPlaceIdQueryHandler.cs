using AutoMapper;
using MediatR;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace MemoryPlaces.Application.Image.Queries.GetAllImagesByPlaceId;

public class GetAllImagesByPlaceIdQueryHandler
    : IRequestHandler<GetAllImagesByPlaceIdQuery, IEnumerable<ImageDataDto>>
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IMapper _mapper;
    private readonly IImageRepository _imageRepository;

    public GetAllImagesByPlaceIdQueryHandler(
        IPlaceRepository placeRepository,
        IMapper mapper,
        IImageRepository imageRepository
    )
    {
        _placeRepository = placeRepository;
        _mapper = mapper;
        _imageRepository = imageRepository;
    }

    public async Task<IEnumerable<ImageDataDto>> Handle(
        GetAllImagesByPlaceIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var place = await _placeRepository.GetByIdAsync(request.PlaceId);

        if (place is null)
        {
            throw new NotFoundException("Place not found");
        }

        var images = await _imageRepository.GetAllByPlaceIdAsync(request.PlaceId);

        if (images.IsNullOrEmpty())
        {
            throw new NotFoundException("Images not found");
        }

        var imageDataDtos = _mapper.Map<IEnumerable<ImageDataDto>>(images);
        return imageDataDtos;
    }
}
