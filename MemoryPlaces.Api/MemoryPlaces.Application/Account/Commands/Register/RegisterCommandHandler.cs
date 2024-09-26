using AutoMapper;
using MediatR;
using MemoryPlaces.Application.Account.Commands.Register;
using MemoryPlaces.Domain.Entities;
using MemoryPlaces.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(
        IAccountRepository accountRepository,
        IMapper mapper,
        IPasswordHasher<User> passwordHasher
    )
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        var hashedPassword = _passwordHasher.HashPassword(user, request.Password);
        user.PasswordHash = hashedPassword;
        await _accountRepository.Create(user);

        return user.Id;
    }
}
