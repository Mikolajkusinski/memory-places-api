using MediatR;
using MemoryPlaces.Application.Place.Commands.Create;
using MemoryPlaces.Application.Place.Commands.Delete;
using MemoryPlaces.Application.Place.Queries.GetAll;
using MemoryPlaces.Application.Place.Queries.GetById;
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

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult> GetAllPlaces([FromQuery] GetAllQuery query)
    {
        var data = await _mediator.Send(query);

        return Ok(data);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult> GetPlaceById([FromQuery] string? locale, string id)
    {
        var query = new GetByIdQuery() { GivenId = id, Locale = locale, };
        var data = await _mediator.Send(query);

        return Ok(data);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePlaceById(string id)
    {
        var query = new DeleteCommand() { Id = id };

        await _mediator.Send(query);

        return NoContent();
    }
}
