using FluentAssertions;
using MemoryPlaces.Application.ApplicationUser;
using MemoryPlaces.Application.Image.Commands.DeleteImage;
using MemoryPlaces.Application.Interfaces;
using MemoryPlaces.Domain.Entities;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;
using Moq;

namespace MemoryPlaces.Application.Tests.Image.Commands;

public class DeleteImageCommandHandlerTests
{
    private readonly Mock<IImageRepository> _imageRepositoryMock;
    private readonly Mock<IBlobStorageService> _blobStorageServiceMock;
    private readonly Mock<IPlaceRepository> _placeRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly DeleteImageCommandHandler _handler;

    public DeleteImageCommandHandlerTests()
    {
        _imageRepositoryMock = new Mock<IImageRepository>();
        _blobStorageServiceMock = new Mock<IBlobStorageService>();
        _placeRepositoryMock = new Mock<IPlaceRepository>();
        _userContextMock = new Mock<IUserContext>();

        _handler = new DeleteImageCommandHandler(
            _imageRepositoryMock.Object,
            _blobStorageServiceMock.Object,
            _placeRepositoryMock.Object,
            _userContextMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenImageDoesNotExist()
    {
        // Arrange
        _imageRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Domain.Entities.Image)null);

        var command = new DeleteImageCommand { ImageId = 1 };

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Image not found");
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenPlaceDoesNotExist()
    {
        // Arrange
        var image = new Domain.Entities.Image { Id = 1, PlaceId = Guid.NewGuid() };
        _imageRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(image);
        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Domain.Entities.Place)null);

        var command = new DeleteImageCommand { ImageId = 1 };

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Place not found");
    }

    [Fact]
    public async Task Handle_ShouldThrowForbidException_WhenUserIsNull()
    {
        // Arrange
        var image = new Domain.Entities.Image { Id = 1, PlaceId = Guid.NewGuid() };
        var place = new Domain.Entities.Place { Id = Guid.NewGuid() };

        _imageRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(image);
        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(place);
        _userContextMock.Setup(ctx => ctx.GetCurrentUser()).Returns((CurrentUser)null);

        var command = new DeleteImageCommand { ImageId = 1 };

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbidException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowForbidException_WhenUserIsNotAdminOrAuthor()
    {
        // Arrange
        var image = new Domain.Entities.Image
        {
            Id = 1,
            PlaceId = Guid.NewGuid(),
            Name = "test.png"
        };
        var place = new Domain.Entities.Place { Id = Guid.NewGuid(), AuthorId = Guid.NewGuid() };
        var user = new CurrentUser("test", "test", "test");

        _imageRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(image);
        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(place);
        _userContextMock.Setup(ctx => ctx.GetCurrentUser()).Returns(user);

        var command = new DeleteImageCommand { ImageId = 1 };

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbidException>();
    }

    [Fact]
    public async Task Handle_ShouldDeleteImageAndCommit_WhenUserIsAuthorized()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var placeId = Guid.NewGuid();

        var image = new Domain.Entities.Image
        {
            Id = 1,
            PlaceId = placeId,
            Name = "test.png"
        };
        var place = new Domain.Entities.Place { Id = placeId, AuthorId = authorId };
        var user = new CurrentUser(authorId.ToString(), "test", "test");

        _imageRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(image);
        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(place);
        _userContextMock.Setup(ctx => ctx.GetCurrentUser()).Returns(user);

        var command = new DeleteImageCommand { ImageId = 1 };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _blobStorageServiceMock.Verify(service => service.DeleteBlobAsync(image.Name), Times.Once);
        _imageRepositoryMock.Verify(repo => repo.Remove(image), Times.Once);
        _imageRepositoryMock.Verify(repo => repo.Commit(), Times.Once);
    }
}
