using FluentAssertions;
using MemoryPlaces.Application.ApplicationUser;
using MemoryPlaces.Application.Interfaces;
using MemoryPlaces.Application.Place.Commands.Verify;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;
using Moq;

namespace MemoryPlaces.Application.Tests.Place.Commands;

public class VerifyCommandHandlerTests
{
    private readonly Mock<IPlaceRepository> _placeRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly VerifyCommandHandler _handler;

    public VerifyCommandHandlerTests()
    {
        _placeRepositoryMock = new Mock<IPlaceRepository>();
        _userContextMock = new Mock<IUserContext>();
        _handler = new VerifyCommandHandler(_placeRepositoryMock.Object, _userContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldVerifyPlace_WhenUserIsAdmin()
    {
        // Arrange
        var place = new Domain.Entities.Place { Id = Guid.NewGuid(), IsVerified = false };
        var user = new CurrentUser(Guid.NewGuid().ToString(), "test@test.com", "Admin");

        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(place.Id.ToString()))
            .ReturnsAsync(place);
        _userContextMock.Setup(ctx => ctx.GetCurrentUser()).Returns(user);

        var verifyCommand = new VerifyCommand { GivenId = place.Id.ToString() };

        // Act
        await _handler.Handle(verifyCommand, CancellationToken.None);

        // Assert
        place.IsVerified.Should().BeTrue();
        place.VerificationDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        _placeRepositoryMock.Verify(repo => repo.Commit(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenPlaceDoesNotExist()
    {
        // Arrange
        var user = new CurrentUser(Guid.NewGuid().ToString(), "test@test.com", "Admin");

        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Domain.Entities.Place?)null);
        _userContextMock.Setup(ctx => ctx.GetCurrentUser()).Returns(user);

        var verifyCommand = new VerifyCommand { GivenId = Guid.NewGuid().ToString() };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(verifyCommand, CancellationToken.None)
        );
        _placeRepositoryMock.Verify(repo => repo.Commit(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowForbidException_WhenUserIsNull()
    {
        // Arrange
        var place = new Domain.Entities.Place { Id = Guid.NewGuid(), IsVerified = false };

        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(place.Id.ToString()))
            .ReturnsAsync(place);
        _userContextMock.Setup(ctx => ctx.GetCurrentUser()).Returns((CurrentUser?)null);

        var verifyCommand = new VerifyCommand { GivenId = place.Id.ToString() };

        // Act & Assert
        await Assert.ThrowsAsync<ForbidException>(
            () => _handler.Handle(verifyCommand, CancellationToken.None)
        );
        _placeRepositoryMock.Verify(repo => repo.Commit(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowForbidException_WhenUserIsNotAdminOrModerator()
    {
        // Arrange
        var place = new Domain.Entities.Place { Id = Guid.NewGuid(), IsVerified = false };

        var user = new CurrentUser(Guid.NewGuid().ToString(), "test@test.com", "User");

        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(place.Id.ToString()))
            .ReturnsAsync(place);
        _userContextMock.Setup(ctx => ctx.GetCurrentUser()).Returns(user);

        var verifyCommand = new VerifyCommand { GivenId = place.Id.ToString() };

        // Act & Assert
        await Assert.ThrowsAsync<ForbidException>(
            () => _handler.Handle(verifyCommand, CancellationToken.None)
        );
        _placeRepositoryMock.Verify(repo => repo.Commit(), Times.Never);
    }
}
