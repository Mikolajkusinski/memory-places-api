using MediatR;
using MemoryPlaces.Application.Place.Commands.Create;
using MemoryPlaces.Application.Place.Commands.Delete;
using MemoryPlaces.Application.Place.Commands.Update;
using MemoryPlaces.Application.Place.Commands.Verify;
using MemoryPlaces.Application.Place.Queries.GetAll;
using MemoryPlaces.Application.Place.Queries.GetAllByUserId;
using MemoryPlaces.Application.Place.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryPlaces.Api.Controllers;

[Route("api/places")]
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
    public async Task<ActionResult> GetAllPlaces(
        [FromQuery] string? locale,
        [FromQuery] string? searchPhrase,
        [FromQuery] int? filterCategoryId,
        [FromQuery] int? filterTypeId,
        [FromQuery] int? filterPeriodId
    )
    {
        var query = new GetAllQuery()
        {
            Locale = locale,
            SearchPhrase = searchPhrase,
            FilterCategoryId = filterCategoryId,
            FilterTypeId = filterTypeId,
            FilterPeriodId = filterPeriodId
        };
        var data = await _mediator.Send(query);

        return Ok(data);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult> GetPlaceById([FromQuery] string? locale, string id)
    {
        var query = new GetByIdQuery() { GivenId = id, Locale = locale };
        var data = await _mediator.Send(query);

        return Ok(data);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeletePlaceById(string id)
    {
        var query = new DeleteCommand() { Id = id };
        await _mediator.Send(query);

        return NoContent();
    }

    [HttpPut("update/{id}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<ActionResult> UpdatePlace([FromQuery] string? locale, string id)
    {
        var query = new UpdateCommand() { GivenId = id, Locale = locale };
        var data = await _mediator.Send(query);

        return Ok(data);
    }

    [HttpPut("verify/{id}")]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<ActionResult> VerifyPlace(string id)
    {
        var query = new VerifyCommand() { GivenId = id };
        await _mediator.Send(query);

        return NoContent();
    }

    [HttpGet("users/{userId}")]
    [AllowAnonymous]
    public async Task<ActionResult> GetPlacesByUserId([FromQuery] string? locale, string userId)
    {
        var query = new GetAllByUserIdQuery() { GivenUserId = userId, Locale = locale };
        var data = await _mediator.Send(query);

        return Ok(data);
    }
}
