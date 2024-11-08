using FluentValidation;

namespace MemoryPlaces.Application.Image.Commands.DeleteImage;

public class DeleteImageCommandValidator : AbstractValidator<DeleteImageCommand>
{
    public DeleteImageCommandValidator()
    {
        RuleFor(x => x.ImageId).NotEmpty().WithMessage("Image id is required");
    }
}
