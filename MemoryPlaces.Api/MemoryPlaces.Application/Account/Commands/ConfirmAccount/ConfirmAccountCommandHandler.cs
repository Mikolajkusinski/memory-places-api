using MediatR;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;

namespace MemoryPlaces.Application.Account.Commands.ConfirmAccount;

public class ConfirmAccountCommandHandler : IRequestHandler<ConfirmAccountCommand>
{
    private readonly IAccountRepository _accountRepository;

    public ConfirmAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task Handle(ConfirmAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _accountRepository.GetUserById(request.Id);

        if (user == null)
        {
            throw new BadRequestException("Account does not exist");
        }

        if (user.IsActive == true)
        {
            throw new BadRequestException("Account already confirmed");
        }

        user.IsConfirmed = true;
        user.IsActive = true;

        await _accountRepository.Commit();
    }
}
