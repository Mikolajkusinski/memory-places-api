using AutoMapper;
using MediatR;
using MemoryPlaces.Domain.RepositoryInterfaces;

namespace MemoryPlaces.Application.Image.Queries.GetAllImages;

public class GetAllImagesQueryHandler
    : IRequestHandler<GetAllImagesQuery, IEnumerable<ImageDataDto>>
{
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;

    public GetAllImagesQueryHandler(IImageRepository imageRepository, IMapper mapper)
    {
        _imageRepository = imageRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ImageDataDto>> Handle(
        GetAllImagesQuery request,
        CancellationToken cancellationToken
    )
    {
        var images = await _imageRepository.GetAllAsync();
        var imageDataDtos = _mapper.Map<IEnumerable<ImageDataDto>>(images);
        return imageDataDtos;
    }
}
