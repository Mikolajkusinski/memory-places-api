using FluentValidation;

namespace MemoryPlaces.Application.Account.Commands.ConfirmAccount;

public class ConfirmAccountCommandValidator : AbstractValidator<ConfirmAccountCommand>
{
    public ConfirmAccountCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
