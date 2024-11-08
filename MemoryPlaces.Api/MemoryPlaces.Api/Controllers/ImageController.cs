using MediatR;
using MemoryPlaces.Application.Extensions;
using MemoryPlaces.Application.Image;
using MemoryPlaces.Application.Image.Commands.DeleteImage;
using MemoryPlaces.Application.Image.Commands.UploadImages;
using MemoryPlaces.Application.Image.Queries.GetAllImages;
using MemoryPlaces.Application.Image.Queries.GetAllImagesByPlaceId;
using MemoryPlaces.Application.Image.Queries.GetImageById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryPlaces.Api.Controllers;

[Route("api/image")]
[ApiController]
[Authorize]
public class ImageController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("place/{placeId}")]
    public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> files, string placeId)
    {
        if (files.Count > 3)
        {
            return BadRequest("You can upload a maximum of 3 files.");
        }

        const long maxFileSize = 2 * 1024 * 1024;
        foreach (var file in files)
        {
            if (file.Length > maxFileSize)
            {
                return BadRequest($"File '{file.FileName}' exceeds the 2 MB size limit.");
            }
        }

        var images = files
            .Select(file =>
            {
                var fileExtension = Path.GetExtension(file.FileName);
                return new ImageDto
                {
                    FileName = $"{Guid.NewGuid()}{fileExtension}",
                    ContentType = file.ContentType,
                    Size = file.Length,
                    Content = file.OpenReadStream().ToByteArray()
                };
            })
            .ToList();

        var command = new UploadImagesCommand(images, placeId);
        var blobUris = await _mediator.Send(command);

        return Ok(blobUris);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteImage(int id)
    {
        var query = new DeleteImageCommand() { ImageId = id };
        await _mediator.Send(query);

        return NoContent();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllImages()
    {
        var query = new GetAllImagesQuery();
        var data = await _mediator.Send(query);
        return Ok(data);
    }

    [HttpGet("place/{placeId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllImagesByPlaceId(string placeId)
    {
        var query = new GetAllImagesByPlaceIdQuery() { PlaceId = placeId };
        var data = await _mediator.Send(query);
        return Ok(data);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetImageById(int id)
    {
        var query = new GetImageByIdQuery() { ImageId = id };
        var data = await _mediator.Send(query);
        return Ok(data);
    }
}
