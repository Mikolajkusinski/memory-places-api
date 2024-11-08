using FluentAssertions;
using MediatR;
using MemoryPlaces.Api.Controllers;
using MemoryPlaces.Application.Account.Commands.ConfirmAccount;
using MemoryPlaces.Application.Account.Commands.Login;
using MemoryPlaces.Application.Account.Commands.Register;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MemoryPlaces.Api.Tests.Controllers;

public class AccountControllerTests
{
    private readonly AccountController _controller;
    private readonly Mock<IMediator> _mediatorMock;

    public AccountControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AccountController(_mediatorMock.Object);
    }

    [Fact]
    public async Task RegisterUser_ShouldReturnOk()
    {
        // Arrange
        var registerCommand = new RegisterCommand
        {
            Name = "TestUser",
            Email = "test@example.com",
            Password = "TestPassword123"
        };

        // Act
        var result = await _controller.RegisterUser(registerCommand);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        _mediatorMock.Verify(m => m.Send(It.IsAny<RegisterCommand>(), default), Times.Once);
    }

    [Fact]
    public async Task Login_ShouldReturnOkWithToken()
    {
        // Arrange
        var loginCommand = new LoginCommand
        {
            Email = "test@example.com",
            Password = "TestPassword123"
        };

        var expectedToken = "test-token";
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<LoginCommand>(), default))
            .ReturnsAsync(expectedToken);

        // Act
        var result = await _controller.Login(loginCommand);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(expectedToken);
        _mediatorMock.Verify(m => m.Send(It.IsAny<LoginCommand>(), default), Times.Once);
    }

    [Fact]
    public async Task Confirm_ShouldReturnOk()
    {
        // Arrange
        var id = "test-id";

        // Act
        var result = await _controller.Confirm(id);

        // Assert
        result.Should().BeOfType<OkResult>();
        _mediatorMock.Verify(m => m.Send(It.IsAny<ConfirmAccountCommand>(), default), Times.Once);
    }
}
