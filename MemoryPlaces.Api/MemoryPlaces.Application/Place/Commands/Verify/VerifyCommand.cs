using MediatR;

namespace MemoryPlaces.Application.Place.Commands.Verify;

public class VerifyCommand : IRequest
{
    public string GivenId { get; set; } = default!;
}
