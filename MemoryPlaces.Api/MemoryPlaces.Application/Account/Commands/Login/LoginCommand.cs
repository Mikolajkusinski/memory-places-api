using MediatR;

namespace MemoryPlaces.Application.Account.Commands.Login;

public class LoginCommand : LoginDto, IRequest<string> { }
