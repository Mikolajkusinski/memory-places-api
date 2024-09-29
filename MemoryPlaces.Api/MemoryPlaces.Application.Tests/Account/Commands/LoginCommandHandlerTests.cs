using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using MemoryPlaces.Application.Account.Commands.Login;
using MemoryPlaces.Application.Settings;
using MemoryPlaces.Domain.Entities;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace MemoryPlaces.Application.Tests.Account.Commands;

public class LoginCommandHandlerTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
    private readonly AuthenticationSettings _authSettings;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();

        _authSettings = new AuthenticationSettings
        {
            JwtKey = "TestJwtKey1234567890TestJwtKey1234567890",
            JwtExpireDays = 1,
            JwtIssuer = "TestIssuer"
        };

        _handler = new LoginCommandHandler(
            _accountRepositoryMock.Object,
            _passwordHasherMock.Object,
            _authSettings
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var command = new LoginCommand { Email = "test@example.com", Password = "password" };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Email = "test@example.com",
            PasswordHash = "hashedPassword",
            IsConfirmed = true,
            IsActive = true,
            Role = new Role { Name = "User" }
        };

        _accountRepositoryMock.Setup(x => x.GetUserByEmail(command.Email)).ReturnsAsync(user);
        _passwordHasherMock
            .Setup(x => x.VerifyHashedPassword(user, user.PasswordHash, command.Password))
            .Returns(PasswordVerificationResult.Success);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNullOrEmpty();
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(result);
        token.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == user.Email);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenUserDoesNotExist()
    {
        // Arrange
        var command = new LoginCommand { Email = "nonexistent@example.com", Password = "password" };

        _accountRepositoryMock.Setup(x => x.GetUserByEmail(command.Email)).ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Invalid username or password");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenPasswordIsIncorrect()
    {
        // Arrange
        var command = new LoginCommand { Email = "test@example.com", Password = "wrongpassword" };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            PasswordHash = "hashedPassword",
            IsConfirmed = true,
            IsActive = true
        };

        _accountRepositoryMock.Setup(x => x.GetUserByEmail(command.Email)).ReturnsAsync(user);
        _passwordHasherMock
            .Setup(x => x.VerifyHashedPassword(user, user.PasswordHash, command.Password))
            .Returns(PasswordVerificationResult.Failed);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Invalid username or password");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenEmailIsNotConfirmed()
    {
        // Arrange
        var command = new LoginCommand { Email = "test@example.com", Password = "password" };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            PasswordHash = "hashedPassword",
            IsConfirmed = false,
            IsActive = true
        };

        _accountRepositoryMock.Setup(x => x.GetUserByEmail(command.Email)).ReturnsAsync(user);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Please confirm your email address");
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenAccountIsInactive()
    {
        // Arrange
        var command = new LoginCommand { Email = "test@example.com", Password = "password" };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            PasswordHash = "hashedPassword",
            IsConfirmed = true,
            IsActive = false
        };

        _accountRepositoryMock.Setup(x => x.GetUserByEmail(command.Email)).ReturnsAsync(user);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Your account is inactive");
    }
}
