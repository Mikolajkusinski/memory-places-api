using FluentValidation;
using MemoryPlaces.Domain.RepositoryInterfaces;

namespace MemoryPlaces.Application.Account.Commands.Register;

public class RegisterDtoCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterDtoCommandValidator(IAccountRepository repository)
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]")
            .WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]")
            .WithMessage("Password must contain at least one special character.");

        RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);

        RuleFor(x => x.Email)
            .Custom(
                (value, context) =>
                {
                    var emailInUse = repository.AccountEmailExist(value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                }
            );

        RuleFor(x => x.Name)
            .Custom(
                (value, context) =>
                {
                    var nameInUse = repository.AccountNameExist(value);
                    if (nameInUse)
                    {
                        context.AddFailure("Name", "That name is taken");
                    }
                }
            );
    }
}
