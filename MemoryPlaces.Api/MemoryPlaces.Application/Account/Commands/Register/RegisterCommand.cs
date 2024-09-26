using MediatR;

namespace MemoryPlaces.Application.Account.Commands.Register;

public class RegisterCommand : RegisterDto, IRequest<Guid> { }
