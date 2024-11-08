using FluentAssertions;
using MemoryPlaces.Application.ApplicationUser;
using MemoryPlaces.Application.Image;
using MemoryPlaces.Application.Image.Commands.UploadImages;
using MemoryPlaces.Application.Interfaces;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;
using Moq;

namespace MemoryPlaces.Application.Tests.Image.Commands;

public class UploadImagesCommandHandlerTests
{
    private readonly Mock<IImageRepository> _imageRepositoryMock;
    private readonly Mock<IBlobStorageService> _blobStorageServiceMock;
    private readonly Mock<IPlaceRepository> _placeRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly UploadImagesCommandHandler _handler;

    public UploadImagesCommandHandlerTests()
    {
        _imageRepositoryMock = new Mock<IImageRepository>();
        _blobStorageServiceMock = new Mock<IBlobStorageService>();
        _placeRepositoryMock = new Mock<IPlaceRepository>();
        _userContextMock = new Mock<IUserContext>();

        _handler = new UploadImagesCommandHandler(
            _imageRepositoryMock.Object,
            _blobStorageServiceMock.Object,
            _placeRepositoryMock.Object,
            _userContextMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenPlaceDoesNotExist()
    {
        // Arrange
        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Domain.Entities.Place)null);
        {
            var images = new List<ImageDto>();
            var command = new UploadImagesCommand(images, "nonexistent-place");
            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Place not found");
        }
    }

    [Fact]
    public async Task Handle_ShouldThrowForbidException_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var place = new Domain.Entities.Place { Id = Guid.NewGuid() };

        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(place);
        _userContextMock.Setup(ctx => ctx.GetCurrentUser()).Returns((CurrentUser)null);
        var images = new List<ImageDto>
        {
            new ImageDto { FileName = "image1.png" },
            new ImageDto { FileName = "image2.png" }
        };

        var command = new UploadImagesCommand(images, place.Id.ToString());

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbidException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowForbidException_WhenUserIsNotAdminOrAuthor()
    {
        // Arrange
        var placeId = Guid.NewGuid();
        var image = new Domain.Entities.Image
        {
            Id = 1,
            PlaceId = placeId,
            Name = "test.png"
        };
        var place = new Domain.Entities.Place { Id = placeId, AuthorId = Guid.NewGuid() };
        var user = new CurrentUser(Guid.NewGuid().ToString(), "test", "Moderator");

        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(place);
        _userContextMock.Setup(ctx => ctx.GetCurrentUser()).Returns(user);

        var images = new List<ImageDto>
        {
            new ImageDto { FileName = "image1.png" },
            new ImageDto { FileName = "image2.png" }
        };

        var command = new UploadImagesCommand(images, place.Id.ToString());

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbidException>();
    }

    [Fact]
    public async Task Handle_ShouldUploadImagesAndReturnBlobUris_WhenUserIsAuthorized()
    {
        // Arrange
        var place = new Domain.Entities.Place { Id = Guid.NewGuid(), AuthorId = Guid.NewGuid() };
        var user = new CurrentUser(place.AuthorId.ToString(), "test", "test");

        var images = new List<ImageDto>
        {
            new ImageDto { FileName = "image1.png" },
            new ImageDto { FileName = "image2.png" }
        };

        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(place);
        _userContextMock.Setup(ctx => ctx.GetCurrentUser()).Returns(user);

        var blobUris = new List<string>
        {
            "https://blobstorage.com/image1.png",
            "https://blobstorage.com/image2.png"
        };
        _blobStorageServiceMock
            .SetupSequence(service => service.UploadBlobAsync(It.IsAny<ImageDto>()))
            .ReturnsAsync(blobUris[0])
            .ReturnsAsync(blobUris[1]);

        var command = new UploadImagesCommand(images, place.Id.ToString());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(blobUris);

        _blobStorageServiceMock.Verify(
            service => service.UploadBlobAsync(It.IsAny<ImageDto>()),
            Times.Exactly(images.Count)
        );
        _imageRepositoryMock.Verify(
            repo => repo.Add(It.IsAny<Domain.Entities.Image>()),
            Times.Exactly(images.Count)
        );
        _imageRepositoryMock.Verify(repo => repo.Commit(), Times.Once);
    }
}
