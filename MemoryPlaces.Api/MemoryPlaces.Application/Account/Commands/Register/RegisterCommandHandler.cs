using AutoMapper;
using MediatR;
using MemoryPlaces.Application.Account.Commands.Register;
using MemoryPlaces.Application.Interfaces;
using MemoryPlaces.Domain.Entities;
using MemoryPlaces.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Guid>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IMapper _mapper;
    private readonly IEmailSenderService _emailSender;
    private readonly ITemplateService _templateService;

    public RegisterCommandHandler(
        IAccountRepository accountRepository,
        IMapper mapper,
        IPasswordHasher<User> passwordHasher,
        IEmailSenderService emailSenderService,
        ITemplateService templateService
    )
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _emailSender = emailSenderService;
        _templateService = templateService;
    }

    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        var hashedPassword = _passwordHasher.HashPassword(user, request.Password);
        user.PasswordHash = hashedPassword;
        await _accountRepository.Create(user);

        var message = await _templateService.LoadConfirmAccountTemplateAsync(user.Id.ToString());
        var test = message;
        await _emailSender.SendEmailAsync(user.Email, "Confirm your account", message);

        return user.Id;
    }
}
