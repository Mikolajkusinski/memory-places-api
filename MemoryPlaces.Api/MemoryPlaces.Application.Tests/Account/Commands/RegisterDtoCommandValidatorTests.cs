using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using MemoryPlaces.Application.Account.Commands.Register;
using MemoryPlaces.Domain.RepositoryInterfaces;
using Moq;

namespace MemoryPlaces.Application.Tests.Account.Commands;

public class RegisterDtoCommandValidatorTests
{
    private readonly Mock<IAccountRepository> _repositoryMock;
    private readonly RegisterDtoCommandValidator _validator;

    public RegisterDtoCommandValidatorTests()
    {
        _repositoryMock = new Mock<IAccountRepository>();

        _validator = new RegisterDtoCommandValidator(_repositoryMock.Object);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var model = new RegisterCommand { Email = string.Empty };
        var result = _validator.TestValidate(model);
        result
            .ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email address.");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new RegisterCommand { Email = "invalid-email" };
        var result = _validator.TestValidate(model);
        result
            .ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email address.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Email_Is_Valid()
    {
        var model = new RegisterCommand { Email = "test@example.com" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var model = new RegisterCommand { Password = string.Empty };
        var result = _validator.TestValidate(model);
        result
            .ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Too_Short()
    {
        var model = new RegisterCommand { Password = "Short1!" };
        var result = _validator.TestValidate(model);
        result
            .ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must be at least 8 characters long.");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Missing_Uppercase()
    {
        var model = new RegisterCommand { Password = "password1!" };
        var result = _validator.TestValidate(model);
        result
            .ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one uppercase letter.");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Missing_Lowercase()
    {
        var model = new RegisterCommand { Password = "PASSWORD1!" };
        var result = _validator.TestValidate(model);
        result
            .ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one lowercase letter.");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Missing_Digit()
    {
        var model = new RegisterCommand { Password = "Password!" };
        var result = _validator.TestValidate(model);
        result
            .ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one digit.");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Missing_Special_Character()
    {
        var model = new RegisterCommand { Password = "Password1" };
        var result = _validator.TestValidate(model);
        result
            .ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must contain at least one special character.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Password_Is_Valid()
    {
        var model = new RegisterCommand { Password = "Password1!" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_ConfirmPassword_Does_Not_Match()
    {
        var model = new RegisterCommand { Password = "Password1!", ConfirmPassword = "Password2!" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
    }

    [Fact]
    public void Should_Not_Have_Error_When_ConfirmPassword_Matches()
    {
        var model = new RegisterCommand { Password = "Password1!", ConfirmPassword = "Password1!" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.ConfirmPassword);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Already_In_Use()
    {
        _repositoryMock.Setup(repo => repo.AccountEmailExist(It.IsAny<string>())).Returns(true);

        var model = new RegisterCommand { Email = "test@example.com" };
        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("That email is taken");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Email_Not_In_Use()
    {
        _repositoryMock.Setup(repo => repo.AccountEmailExist(It.IsAny<string>())).Returns(false);

        var model = new RegisterCommand { Email = "test@example.com" };
        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Already_In_Use()
    {
        _repositoryMock.Setup(repo => repo.AccountNameExist(It.IsAny<string>())).Returns(true);

        var model = new RegisterCommand { Name = "TestUser" };
        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("That name is taken");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Name_Not_In_Use()
    {
        _repositoryMock.Setup(repo => repo.AccountNameExist(It.IsAny<string>())).Returns(false);

        var model = new RegisterCommand { Name = "TestUser" };
        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}
