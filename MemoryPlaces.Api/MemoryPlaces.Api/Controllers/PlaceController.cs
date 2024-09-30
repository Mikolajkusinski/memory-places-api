using MediatR;
using MemoryPlaces.Application.Place.Commands.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryPlaces.Api.Controllers;

[Route("api/place")]
[ApiController]
[Authorize]
public class PlaceController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlaceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreatePlace([FromBody] CreateCommand command)
    {
        var PlaceId = await _mediator.Send(command);
        return Created(PlaceId, null);
    }
}
