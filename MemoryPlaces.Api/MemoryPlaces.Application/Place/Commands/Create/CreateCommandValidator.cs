using FluentValidation;

namespace MemoryPlaces.Application.Place.Commands.Create;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(50);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(500);

        RuleFor(x => x.Latitude)
            .NotEmpty()
            .WithMessage("Latitude is required")
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90 degrees.");
        ;

        RuleFor(x => x.Longitude)
            .NotEmpty()
            .WithMessage("Longitude is required")
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -90 and 90 degrees.");
        ;

        RuleFor(x => x.WikipediaLink)
            .Must(BeAValidUrl)
            .When(x => !string.IsNullOrEmpty(x.WikipediaLink))
            .WithMessage("Wikipedia link must be a valid URL.");

        RuleFor(x => x.WebsiteLink)
            .Must(BeAValidUrl)
            .When(x => !string.IsNullOrEmpty(x.WebsiteLink))
            .WithMessage("Website link must be a valid URL.");

        RuleFor(x => x.AuthorId)
            .Must(id => id != Guid.Empty)
            .WithMessage("AuthorId must be a valid GUID.");

        RuleFor(x => x.TypeId).GreaterThan(0).WithMessage("TypeId must be greater than 0.");
        RuleFor(x => x.PeriodId).GreaterThan(0).WithMessage("PeriodId must be greater than 0.");
        RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("CategoryId must be greater than 0.");
    }

    private bool BeAValidUrl(string? link)
    {
        if (Uri.TryCreate(link, UriKind.Absolute, out Uri? uriResult))
        {
            return uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps;
        }
        return false;
    }
}
