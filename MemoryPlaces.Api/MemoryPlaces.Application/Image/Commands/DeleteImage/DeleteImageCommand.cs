using MediatR;

namespace MemoryPlaces.Application.Image.Commands.DeleteImage;

public class DeleteImageCommand : IRequest
{
    public int ImageId { get; set; }
}
