using AutoMapper;
using FluentAssertions;
using MemoryPlaces.Application.Image;
using MemoryPlaces.Application.Image.Queries.GetAllImages;
using MemoryPlaces.Domain.RepositoryInterfaces;
using Moq;

namespace MemoryPlaces.Application.Tests.Image.Queries;

public class GetAllImagesQueryHandlerTests
{
    private readonly Mock<IImageRepository> _imageRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllImagesQueryHandler _handler;

    public GetAllImagesQueryHandlerTests()
    {
        _imageRepositoryMock = new Mock<IImageRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetAllImagesQueryHandler(_imageRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedImageDataDtos_WhenImagesExist()
    {
        // Arrange
        var images = new List<Domain.Entities.Image>
        {
            new()
            {
                Id = 1,
                Name = "Image1",
                ImagePath = "path1.jpg"
            },
            new()
            {
                Id = 2,
                Name = "Image2",
                ImagePath = "path2.jpg"
            }
        };

        var imageDataDtos = new List<ImageDataDto>
        {
            new ImageDataDto
            {
                Id = 1,
                Name = "Image1",
                ImagePath = "path1.jpg"
            },
            new ImageDataDto
            {
                Id = 2,
                Name = "Image2",
                ImagePath = "path2.jpg"
            }
        };

        _imageRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(images);

        _mapperMock
            .Setup(mapper => mapper.Map<IEnumerable<ImageDataDto>>(images))
            .Returns(imageDataDtos);

        var query = new GetAllImagesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(imageDataDtos);
        _imageRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<IEnumerable<ImageDataDto>>(images), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoImagesExist()
    {
        // Arrange
        _imageRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Domain.Entities.Image>());

        _mapperMock
            .Setup(mapper =>
                mapper.Map<IEnumerable<ImageDataDto>>(It.IsAny<List<Domain.Entities.Image>>())
            )
            .Returns(Enumerable.Empty<ImageDataDto>());

        var query = new GetAllImagesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
        _imageRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mapperMock.Verify(
            mapper =>
                mapper.Map<IEnumerable<ImageDataDto>>(It.IsAny<List<Domain.Entities.Image>>()),
            Times.Once
        );
    }
}
