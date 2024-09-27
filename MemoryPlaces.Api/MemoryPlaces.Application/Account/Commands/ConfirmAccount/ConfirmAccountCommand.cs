using MediatR;

namespace MemoryPlaces.Application.Account.Commands.ConfirmAccount;

public class ConfirmAccountCommand : IRequest
{
    public string Id { get; set; } = default!;
}
