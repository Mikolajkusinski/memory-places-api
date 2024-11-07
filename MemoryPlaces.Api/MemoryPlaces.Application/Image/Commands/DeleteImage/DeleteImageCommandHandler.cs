using MediatR;
using MemoryPlaces.Application.Interfaces;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;

namespace MemoryPlaces.Application.Image.Commands.DeleteImage;

public class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand>
{
    private readonly IImageRepository _imageRepository;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IPlaceRepository _placeRepository;
    private readonly IUserContext _userContext;

    public DeleteImageCommandHandler(
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

    public async Task Handle(DeleteImageCommand request, CancellationToken cancellationToken)
    {
        var user = _userContext.GetCurrentUser();
        var image = await _imageRepository.GetByIdAsync(request.ImageId);

        if (image is null)
        {
            throw new NotFoundException("Image not found");
        }

        var place = await _placeRepository.GetByIdAsync(image.PlaceId.ToString());

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

        await _blobStorageService.DeleteBlobAsync(image.Name);

        _imageRepository.Remove(image);
        await _imageRepository.Commit();
    }
}
