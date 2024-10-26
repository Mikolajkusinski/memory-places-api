using FluentValidation.TestHelper;
using MemoryPlaces.Application.Place.Commands.Create;

namespace MemoryPlaces.Application.Tests.Place.Commands;

public class CreateCommandValidatorTests
{
    private readonly CreateCommandValidator _validator;

    public CreateCommandValidatorTests()
    {
        _validator = new CreateCommandValidator();
    }

    [Fact]
    public void Should_HaveError_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateCommand { Name = "" };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("Name is required");
    }

    [Fact]
    public void Should_HaveError_WhenNameIsTooLong()
    {
        // Arrange
        var command = new CreateCommand { Name = new string('a', 51) };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_HaveError_WhenDescriptionIsEmpty()
    {
        // Arrange
        var command = new CreateCommand { Description = "" };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result
            .ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description is required");
    }

    [Fact]
    public void Should_HaveError_WhenDescriptionIsTooLong()
    {
        // Arrange
        var command = new CreateCommand { Description = new string('a', 501) };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_HaveError_WhenLatitudeIsOutOfRange()
    {
        // Arrange
        var command = new CreateCommand { Latitude = 100m };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result
            .ShouldHaveValidationErrorFor(x => x.Latitude)
            .WithErrorMessage("Latitude must be between -90 and 90 degrees.");
    }

    [Fact]
    public void Should_HaveError_WhenLongitudeIsOutOfRange()
    {
        // Arrange
        var command = new CreateCommand { Longitude = 200m };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result
            .ShouldHaveValidationErrorFor(x => x.Longitude)
            .WithErrorMessage("Longitude must be between -90 and 90 degrees.");
    }

    [Fact]
    public void Should_HaveError_WhenWikipediaLinkIsInvalidUrl()
    {
        // Arrange
        var command = new CreateCommand { WikipediaLink = "invalid-url" };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result
            .ShouldHaveValidationErrorFor(x => x.WikipediaLink)
            .WithErrorMessage("Wikipedia link must be a valid URL.");
    }

    [Fact]
    public void Should_HaveError_WhenWebsiteLinkIsInvalidUrl()
    {
        // Arrange
        var command = new CreateCommand { WebsiteLink = "invalid-url" };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result
            .ShouldHaveValidationErrorFor(x => x.WebsiteLink)
            .WithErrorMessage("Website link must be a valid URL.");
    }

    [Fact]
    public void Should_HaveError_WhenAuthorIdIsEmptyGuid()
    {
        // Arrange
        var command = new CreateCommand { AuthorId = Guid.Empty };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result
            .ShouldHaveValidationErrorFor(x => x.AuthorId)
            .WithErrorMessage("AuthorId must be a valid GUID.");
    }

    [Fact]
    public void Should_HaveError_WhenTypeIdIsZeroOrNegative()
    {
        // Arrange
        var command = new CreateCommand { TypeId = 0 };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result
            .ShouldHaveValidationErrorFor(x => x.TypeId)
            .WithErrorMessage("TypeId must be greater than 0.");
    }

    [Fact]
    public void Should_HaveError_WhenPeriodIdIsZeroOrNegative()
    {
        // Arrange
        var command = new CreateCommand { PeriodId = 0 };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result
            .ShouldHaveValidationErrorFor(x => x.PeriodId)
            .WithErrorMessage("PeriodId must be greater than 0.");
    }

    [Fact]
    public void Should_HaveError_WhenCategoryIdIsZeroOrNegative()
    {
        // Arrange
        var command = new CreateCommand { CategoryId = 0 };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result
            .ShouldHaveValidationErrorFor(x => x.CategoryId)
            .WithErrorMessage("CategoryId must be greater than 0.");
    }

    [Fact]
    public void Should_NotHaveError_ForValidCommand()
    {
        // Arrange
        var command = new CreateCommand
        {
            Name = "Test Place",
            Description = "Test Description",
            Latitude = 51.5074m,
            Longitude = -0.1278m,
            WikipediaLink = "https://en.wikipedia.org/wiki/Test_Place",
            WebsiteLink = "https://testplace.com",
            AuthorId = Guid.NewGuid(),
            TypeId = 1,
            PeriodId = 1,
            CategoryId = 1
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
