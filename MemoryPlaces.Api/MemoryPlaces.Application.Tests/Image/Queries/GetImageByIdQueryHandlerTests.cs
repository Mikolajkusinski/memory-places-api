using AutoMapper;
using FluentAssertions;
using MemoryPlaces.Application.Image;
using MemoryPlaces.Application.Image.Queries.GetImageById;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;
using Moq;

namespace MemoryPlaces.Application.Tests.Image.Queries;

public class GetImageByIdQueryHandlerTests
{
    private readonly Mock<IImageRepository> _imageRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetImageByIdQueryHandler _handler;

    public GetImageByIdQueryHandlerTests()
    {
        _imageRepositoryMock = new Mock<IImageRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetImageByIdQueryHandler(_mapperMock.Object, _imageRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenImageDoesNotExist()
    {
        // Arrange
        var query = new GetImageByIdQuery { ImageId = 1 };

        _imageRepositoryMock
            .Setup(repo => repo.GetByIdAsync(query.ImageId))
            .ReturnsAsync((Domain.Entities.Image)null);

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Image not found");
        _imageRepositoryMock.Verify(repo => repo.GetByIdAsync(query.ImageId), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedImageDataDto_WhenImageExists()
    {
        // Arrange
        var query = new GetImageByIdQuery { ImageId = 1 };

        var image = new Domain.Entities.Image
        {
            Id = 1,
            Name = "Image1",
            ImagePath = "path1.jpg",
            PlaceId = Guid.NewGuid()
        };

        var imageDataDto = new ImageDataDto
        {
            Id = image.Id,
            Name = image.Name,
            ImagePath = image.ImagePath
        };

        _imageRepositoryMock.Setup(repo => repo.GetByIdAsync(query.ImageId)).ReturnsAsync(image);

        _mapperMock.Setup(mapper => mapper.Map<ImageDataDto>(image)).Returns(imageDataDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(imageDataDto);
        _imageRepositoryMock.Verify(repo => repo.GetByIdAsync(query.ImageId), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<ImageDataDto>(image), Times.Once);
    }
}
