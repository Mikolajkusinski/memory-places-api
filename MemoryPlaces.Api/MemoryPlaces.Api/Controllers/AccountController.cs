using MediatR;
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
}
