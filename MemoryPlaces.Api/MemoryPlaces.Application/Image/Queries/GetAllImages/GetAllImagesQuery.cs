using MediatR;

namespace MemoryPlaces.Application.Image.Queries.GetAllImages;

public class GetAllImagesQuery : IRequest<IEnumerable<ImageDataDto>> { }
