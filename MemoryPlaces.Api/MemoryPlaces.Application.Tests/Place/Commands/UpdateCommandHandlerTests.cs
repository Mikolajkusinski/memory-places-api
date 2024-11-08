using AutoMapper;
using FluentAssertions;
using MemoryPlaces.Application.ApplicationUser;
using MemoryPlaces.Application.Interfaces;
using MemoryPlaces.Application.Place;
using MemoryPlaces.Application.Place.Commands.Update;
using MemoryPlaces.Domain.Entities;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;
using Moq;

namespace MemoryPlaces.Application.Tests.Place.Commands;

public class UpdateCommandHandlerTests
{
    private readonly Mock<IPlaceRepository> _placeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly UpdateCommandHandler _handler;

    public UpdateCommandHandlerTests()
    {
        _placeRepositoryMock = new Mock<IPlaceRepository>();
        _mapperMock = new Mock<IMapper>();
        _userContextMock = new Mock<IUserContext>();
        _handler = new UpdateCommandHandler(
            _placeRepositoryMock.Object,
            _mapperMock.Object,
            _userContextMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldUpdatePlace_WhenUserIsAdmin()
    {
        // Arrange
        var place = new Domain.Entities.Place { Id = Guid.NewGuid(), AuthorId = Guid.NewGuid() };
        var user = new CurrentUser(Guid.NewGuid().ToString(), "test@test.com", "Admin");

        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(place.Id.ToString()))
            .ReturnsAsync(place);
        _userContextMock.Setup(ctx => ctx.GetCurrentUser()).Returns(user);

        var updateCommand = new UpdateCommand
        {
            GivenId = place.Id.ToString(),
            Name = "Updated Name",
            Locale = "en"
        };
        var expectedPlaceDto = new PlaceDto { Id = place.Id, Name = "Updated Name" };

        _mapperMock
            .Setup(mapper =>
                mapper.Map<PlaceDto>(
                    It.IsAny<Domain.Entities.Place>(),
                    It.IsAny<Action<IMappingOperationOptions<object, PlaceDto>>>()
                )
            )
            .Returns(expectedPlaceDto);

        // Act
        var result = await _handler.Handle(updateCommand, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedPlaceDto);
        _placeRepositoryMock.Verify(repo => repo.Commit(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldUpdatePlace_WhenUserIsAuthor()
    {
        // Arrange
        var placeId = Guid.NewGuid();
        var authorId = Guid.Parse("6B29FC40-CA47-1067-B31D-00DD010662DA");
        var place = new Domain.Entities.Place
        {
            Id = placeId,
            AuthorId = authorId,
            Name = "Original Name",
            Description = "Original Description"
        };
        var user = new CurrentUser(authorId.ToString(), "test@test.com", "User");

        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(place.Id.ToString()))
            .ReturnsAsync(place);

        _userContextMock.Setup(ctx => ctx.GetCurrentUser()).Returns(user);

        var updateCommand = new UpdateCommand
        {
            GivenId = place.Id.ToString(),
            Name = "Updated Name",
            Locale = "en"
        };
        var expectedPlaceDto = new PlaceDto { Id = place.Id, Name = "Updated Name" };

        _mapperMock
            .Setup(mapper =>
                mapper.Map<PlaceDto>(
                    It.IsAny<Domain.Entities.Place>(),
                    It.IsAny<Action<IMappingOperationOptions<object, PlaceDto>>>()
                )
            )
            .Returns(expectedPlaceDto);

        // Act
        var result = await _handler.Handle(updateCommand, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedPlaceDto);
        _placeRepositoryMock.Verify(repo => repo.Commit(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowForbidException_WhenUserIsNotAdminOrAuthor()
    {
        // Arrange
        var place = new Domain.Entities.Place { Id = Guid.NewGuid(), AuthorId = Guid.NewGuid() };
        var user = new CurrentUser(Guid.NewGuid().ToString(), "test@test.com", "Moderator");

        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(place.Id.ToString()))
            .ReturnsAsync(place);
        _userContextMock.Setup(ctx => ctx.GetCurrentUser()).Returns(user);

        var updateCommand = new UpdateCommand { GivenId = place.Id.ToString() };

        // Act & Assert
        await Assert.ThrowsAsync<ForbidException>(
            () => _handler.Handle(updateCommand, CancellationToken.None)
        );
        _placeRepositoryMock.Verify(repo => repo.Commit(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenPlaceNotFound()
    {
        // Arrange
        var updateCommand = new UpdateCommand { GivenId = Guid.NewGuid().ToString() };
        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>().ToString()))
            .ReturnsAsync((Domain.Entities.Place?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(updateCommand, CancellationToken.None)
        );
        _placeRepositoryMock.Verify(repo => repo.Commit(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowForbidException_WhenNoUserInContext()
    {
        // Arrange
        var place = new Domain.Entities.Place { Id = Guid.NewGuid(), AuthorId = Guid.NewGuid() };
        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(place.Id.ToString()))
            .ReturnsAsync(place);
        _userContextMock.Setup(ctx => ctx.GetCurrentUser()).Returns((CurrentUser?)null);

        var updateCommand = new UpdateCommand { GivenId = place.Id.ToString() };

        // Act & Assert
        await Assert.ThrowsAsync<ForbidException>(
            () => _handler.Handle(updateCommand, CancellationToken.None)
        );
        _placeRepositoryMock.Verify(repo => repo.Commit(), Times.Never);
    }
}
