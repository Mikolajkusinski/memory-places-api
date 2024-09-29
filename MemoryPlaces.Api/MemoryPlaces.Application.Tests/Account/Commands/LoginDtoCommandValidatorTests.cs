using FluentAssertions;
using FluentValidation.TestHelper;
using MemoryPlaces.Application.Account.Commands.Login;

namespace MemoryPlaces.Application.Tests.Account.Commands;

public class LoginDtoCommandValidatorTests
{
    private readonly LoginDtoCommandValidator _validator;

    public LoginDtoCommandValidatorTests()
    {
        _validator = new LoginDtoCommandValidator();
    }

    [Fact]
    public void Should_HaveNoValidationErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "test@example.com",
            Password = "ValidPassword123"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_HaveValidationError_WhenEmailIsMissing()
    {
        // Arrange
        var command = new LoginCommand { Email = "", Password = "ValidPassword123" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email).WithErrorMessage("Email is required.");
    }

    [Fact]
    public void Should_HaveValidationError_WhenEmailIsInvalid()
    {
        // Arrange
        var command = new LoginCommand { Email = "invalid-email", Password = "ValidPassword123" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Email)
            .WithErrorMessage("Invalid email address.");
    }

    [Fact]
    public void Should_HaveValidationError_WhenPasswordIsMissing()
    {
        // Arrange
        var command = new LoginCommand { Email = "test@example.com", Password = "" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Password)
            .WithErrorMessage("Password is required.");
    }
}
