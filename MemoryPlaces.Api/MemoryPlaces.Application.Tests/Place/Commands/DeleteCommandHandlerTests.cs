using AutoMapper;
using MemoryPlaces.Application.Place.Commands.Delete;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;
using Moq;

namespace MemoryPlaces.Application.Tests.Place.Commands;

public class DeleteCommandHandlerTests
{
    private readonly Mock<IPlaceRepository> _placeRepositoryMock;
    private readonly DeleteCommandHandler _handler;

    public DeleteCommandHandlerTests()
    {
        _placeRepositoryMock = new Mock<IPlaceRepository>();
        _handler = new DeleteCommandHandler(_placeRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeletePlace_WhenPlaceExists()
    {
        // Arrange
        var placeId = Guid.NewGuid();
        var place = new Domain.Entities.Place { Id = placeId };

        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(placeId.ToString()))
            .ReturnsAsync(place);

        var command = new DeleteCommand { Id = placeId.ToString() };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _placeRepositoryMock.Verify(repo => repo.GetByIdAsync(placeId.ToString()), Times.Once);
        _placeRepositoryMock.Verify(repo => repo.Remove(place), Times.Once);
        _placeRepositoryMock.Verify(repo => repo.Commit(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenPlaceDoesNotExist()
    {
        // Arrange
        var placeId = Guid.NewGuid();
        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(placeId.ToString()))
            .ReturnsAsync((Domain.Entities.Place?)null);

        var command = new DeleteCommand { Id = placeId.ToString() };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None)
        );

        _placeRepositoryMock.Verify(repo => repo.GetByIdAsync(placeId.ToString()), Times.Once);
        _placeRepositoryMock.Verify(
            repo => repo.Remove(It.IsAny<Domain.Entities.Place>()),
            Times.Never
        );
        _placeRepositoryMock.Verify(repo => repo.Commit(), Times.Never);
    }
}
