using AutoMapper;
using MediatR;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;

namespace MemoryPlaces.Application.Image.Queries.GetImageById;

public class GetImageByIdQueryHandler : IRequestHandler<GetImageByIdQuery, ImageDataDto>
{
    private readonly IMapper _mapper;
    private readonly IImageRepository _imageRepository;

    public GetImageByIdQueryHandler(IMapper mapper, IImageRepository imageRepository)
    {
        _mapper = mapper;
        _imageRepository = imageRepository;
    }

    public async Task<ImageDataDto> Handle(
        GetImageByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var image = await _imageRepository.GetByIdAsync(request.ImageId);

        if (image is null)
        {
            throw new NotFoundException("Image not found");
        }

        var imageDataDto = _mapper.Map<ImageDataDto>(image);

        return imageDataDto;
    }
}
