using FluentValidation;

namespace MemoryPlaces.Application.Place.Commands.Verify;

public class VerifyCommandValidator : AbstractValidator<VerifyCommand>
{
    public VerifyCommandValidator()
    {
        RuleFor(x => x.GivenId).NotEmpty().WithMessage("Id is required");
    }
}
