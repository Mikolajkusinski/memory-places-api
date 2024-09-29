using AutoMapper;
using FluentAssertions;
using MemoryPlaces.Application.Account.Commands.Register;
using MemoryPlaces.Application.Interfaces;
using MemoryPlaces.Domain.Entities;
using MemoryPlaces.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Moq;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IEmailSenderService> _emailSenderMock;
    private readonly Mock<ITemplateService> _templateServiceMock;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        _mapperMock = new Mock<IMapper>();
        _emailSenderMock = new Mock<IEmailSenderService>();
        _templateServiceMock = new Mock<ITemplateService>();

        _handler = new RegisterCommandHandler(
            _accountRepositoryMock.Object,
            _mapperMock.Object,
            _passwordHasherMock.Object,
            _emailSenderMock.Object,
            _templateServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldRegisterUser_AndSendConfirmationEmail()
    {
        // Arrange
        var registerCommand = new RegisterCommand
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "Password123",
            RoleId = 1
        };

        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Email = registerCommand.Email };

        _mapperMock.Setup(m => m.Map<User>(It.IsAny<RegisterCommand>())).Returns(user);
        _passwordHasherMock
            .Setup(m => m.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
            .Returns("hashed_password");

        _accountRepositoryMock.Setup(a => a.Create(It.IsAny<User>())).Returns(Task.CompletedTask);

        _templateServiceMock
            .Setup(t => t.LoadConfirmAccountTemplateAsync(userId.ToString()))
            .ReturnsAsync("Email body with @activationLink");

        _emailSenderMock
            .Setup(e =>
                e.SendEmailAsync(
                    user.Email,
                    "Confirm your account",
                    "Email body with @activationLink"
                )
            )
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(registerCommand, CancellationToken.None);

        // Assert
        result.Should().Be(userId);
        _accountRepositoryMock.Verify(a => a.Create(user), Times.Once);
        _passwordHasherMock.Verify(p => p.HashPassword(user, registerCommand.Password), Times.Once);
        _emailSenderMock.Verify(
            e => e.SendEmailAsync(user.Email, "Confirm your account", It.IsAny<string>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserCreationFails()
    {
        // Arrange
        var registerCommand = new RegisterCommand
        {
            Name = "Jane Doe",
            Email = "jane.doe@example.com",
            Password = "Password123",
            RoleId = 1
        };

        var user = new User { Email = registerCommand.Email };

        _mapperMock.Setup(m => m.Map<User>(It.IsAny<RegisterCommand>())).Returns(user);
        _passwordHasherMock
            .Setup(m => m.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
            .Returns("hashed_password");

        _accountRepositoryMock
            .Setup(a => a.Create(It.IsAny<User>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        Func<Task> act = async () => await _handler.Handle(registerCommand, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
        _accountRepositoryMock.Verify(a => a.Create(user), Times.Once);
    }
}
