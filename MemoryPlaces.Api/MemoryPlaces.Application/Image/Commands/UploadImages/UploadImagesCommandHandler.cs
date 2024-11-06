using MediatR;
using MemoryPlaces.Application.Interfaces;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;

namespace MemoryPlaces.Application.Image.Commands.UploadImages;

public class UploadImagesCommandHandler : IRequestHandler<UploadImagesCommand, IEnumerable<string>>
{
    private readonly IImageRepository _imageRepository;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IPlaceRepository _placeRepository;
    private readonly IUserContext _userContext;

    public UploadImagesCommandHandler(
        IImageRepository imageRepository,
        IBlobStorageService blobStorageService,
        IPlaceRepository placeRepository,
        IUserContext userContext
    )
    {
        _imageRepository = imageRepository;
        _blobStorageService = blobStorageService;
        _placeRepository = placeRepository;
        _userContext = userContext;
    }

    public async Task<IEnumerable<string>> Handle(
        UploadImagesCommand request,
        CancellationToken cancellationToken
    )
    {
        var place = await _placeRepository.GetByIdAsync(request.PlaceId);
        var user = _userContext.GetCurrentUser();

        if (place is null)
        {
            throw new NotFoundException("Place not found");
        }

        if (user is null)
        {
            throw new ForbidException();
        }

        var isAdmin = user!.IsInRole("Admin");
        var isAuthor = place.AuthorId.ToString() == user.Id;

        if (!isAdmin && !isAuthor)
        {
            throw new ForbidException();
        }

        var blobUris = new List<string>();

        foreach (var image in request.Images)
        {
            var blobUri = await _blobStorageService.UploadBlobAsync(image);
            _imageRepository.Add(
                new Domain.Entities.Image
                {
                    Name = image.FileName,
                    ImagePath = blobUri,
                    PlaceId = Guid.Parse(request.PlaceId)
                }
            );
            blobUris.Add(blobUri);
        }

        await _imageRepository.Commit();

        return blobUris;
    }
}
