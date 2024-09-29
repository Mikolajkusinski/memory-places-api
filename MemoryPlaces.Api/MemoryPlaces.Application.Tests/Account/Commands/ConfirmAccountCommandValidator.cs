using FluentValidation.TestHelper;
using MemoryPlaces.Application.Account.Commands.ConfirmAccount;

namespace MemoryPlaces.Application.Tests.Account.Commands;

public class ConfirmAccountCommandValidatorTests
{
    private readonly ConfirmAccountCommandValidator _validator;

    public ConfirmAccountCommandValidatorTests()
    {
        _validator = new ConfirmAccountCommandValidator();
    }

    [Fact]
    public void Validator_ShouldNotHaveValidationError_WhenIdIsProvided()
    {
        // Arrange
        var command = new ConfirmAccountCommand { Id = Guid.NewGuid().ToString() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validator_ShouldHaveValidationError_WhenIdIsEmpty()
    {
        // Arrange
        var command = new ConfirmAccountCommand { Id = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id).WithErrorMessage("Id is required");
    }
}
