using FluentAssertions;
using MemoryPlaces.Application.Account.Commands.ConfirmAccount;
using MemoryPlaces.Domain.Entities;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;
using Moq;

namespace MemoryPlaces.Application.Tests.Account.Commands;

public class ConfirmAccountCommandHandlerTests
{
    private readonly ConfirmAccountCommandHandler _handler;
    private readonly Mock<IAccountRepository> _accountRepositoryMock;

    public ConfirmAccountCommandHandlerTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _handler = new ConfirmAccountCommandHandler(_accountRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequestException_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new ConfirmAccountCommand { Id = Guid.NewGuid().ToString() };
        _accountRepositoryMock
            .Setup(repo => repo.GetUserById(It.IsAny<string>()))
            .ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>().WithMessage("Account does not exist");
        _accountRepositoryMock.Verify(repo => repo.GetUserById(It.IsAny<string>()), Times.Once);
        _accountRepositoryMock.Verify(repo => repo.Commit(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowBadRequestException_WhenUserIsAlreadyConfirmed()
    {
        // Arrange
        var command = new ConfirmAccountCommand { Id = Guid.NewGuid().ToString() };
        var user = new User
        {
            Id = Guid.Parse(command.Id),
            IsConfirmed = true,
            IsActive = true
        };

        _accountRepositoryMock
            .Setup(repo => repo.GetUserById(It.IsAny<string>()))
            .ReturnsAsync(user);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Account already confirmed");
        _accountRepositoryMock.Verify(repo => repo.GetUserById(It.IsAny<string>()), Times.Once);
        _accountRepositoryMock.Verify(repo => repo.Commit(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldConfirmAccount_WhenUserIsNotConfirmed()
    {
        // Arrange
        var command = new ConfirmAccountCommand { Id = Guid.NewGuid().ToString() };
        var user = new User
        {
            Id = Guid.Parse(command.Id),
            IsConfirmed = false,
            IsActive = false
        };

        _accountRepositoryMock
            .Setup(repo => repo.GetUserById(It.IsAny<string>()))
            .ReturnsAsync(user);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        user.IsConfirmed.Should().BeTrue();
        user.IsActive.Should().BeTrue();
        _accountRepositoryMock.Verify(repo => repo.GetUserById(It.IsAny<string>()), Times.Once);
        _accountRepositoryMock.Verify(repo => repo.Commit(), Times.Once);
    }
}
