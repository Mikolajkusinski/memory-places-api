using MediatR;
using MemoryPlaces.Application.Account.Commands.ConfirmAccount;
using MemoryPlaces.Application.Account.Commands.Login;
using MemoryPlaces.Application.Account.Commands.Register;
using Microsoft.AspNetCore.Mvc;

namespace MemoryPlaces.Api.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginCommand command)
    {
        var token = await _mediator.Send(command);
        return Ok(token);
    }

    [HttpPost("confirm/{id}")]
    public async Task<ActionResult> Confirm(string id)
    {
        await _mediator.Send(new ConfirmAccountCommand() { Id = id });
        return Ok();
    }
}
