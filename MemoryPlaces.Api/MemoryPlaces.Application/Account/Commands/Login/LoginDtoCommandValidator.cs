using FluentValidation;

namespace MemoryPlaces.Application.Account.Commands.Login;

public class LoginDtoCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginDtoCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email address.");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
    }
}
