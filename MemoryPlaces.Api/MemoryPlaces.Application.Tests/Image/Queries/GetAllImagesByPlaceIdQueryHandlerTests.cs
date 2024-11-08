using AutoMapper;
using FluentAssertions;
using MemoryPlaces.Application.Image;
using MemoryPlaces.Application.Image.Queries.GetAllImagesByPlaceId;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;
using Moq;

namespace MemoryPlaces.Application.Tests.Image.Queries;

public class GetAllImagesByPlaceIdQueryHandlerTests
{
    private readonly Mock<IPlaceRepository> _placeRepositoryMock;
    private readonly Mock<IImageRepository> _imageRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllImagesByPlaceIdQueryHandler _handler;

    public GetAllImagesByPlaceIdQueryHandlerTests()
    {
        _placeRepositoryMock = new Mock<IPlaceRepository>();
        _imageRepositoryMock = new Mock<IImageRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetAllImagesByPlaceIdQueryHandler(
            _placeRepositoryMock.Object,
            _mapperMock.Object,
            _imageRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenPlaceDoesNotExist()
    {
        // Arrange
        var query = new GetAllImagesByPlaceIdQuery { PlaceId = "nonexistent-id" };

        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(query.PlaceId))
            .ReturnsAsync((Domain.Entities.Place)null);

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Place not found");
        _placeRepositoryMock.Verify(repo => repo.GetByIdAsync(query.PlaceId), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenNoImagesExistForPlace()
    {
        // Arrange
        var query = new GetAllImagesByPlaceIdQuery { PlaceId = Guid.NewGuid().ToString() };

        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(query.PlaceId))
            .ReturnsAsync(new Domain.Entities.Place { Id = Guid.Parse(query.PlaceId) });

        _imageRepositoryMock
            .Setup(repo => repo.GetAllByPlaceIdAsync(query.PlaceId))
            .ReturnsAsync(new List<Domain.Entities.Image>());

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Images not found");
        _placeRepositoryMock.Verify(repo => repo.GetByIdAsync(query.PlaceId), Times.Once);
        _imageRepositoryMock.Verify(repo => repo.GetAllByPlaceIdAsync(query.PlaceId), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedImageDataDtos_WhenImagesExistForPlace()
    {
        // Arrange
        var query = new GetAllImagesByPlaceIdQuery { PlaceId = Guid.NewGuid().ToString() };

        var images = new List<Domain.Entities.Image>
        {
            new()
            {
                Id = 1,
                Name = "Image1",
                ImagePath = "path1.jpg",
                PlaceId = Guid.Parse(query.PlaceId)
            },
            new()
            {
                Id = 2,
                Name = "Image2",
                ImagePath = "path2.jpg",
                PlaceId = Guid.Parse(query.PlaceId)
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

        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(query.PlaceId))
            .ReturnsAsync(new Domain.Entities.Place { Id = Guid.Parse(query.PlaceId) });

        _imageRepositoryMock
            .Setup(repo => repo.GetAllByPlaceIdAsync(query.PlaceId))
            .ReturnsAsync(images);

        _mapperMock
            .Setup(mapper => mapper.Map<IEnumerable<ImageDataDto>>(images))
            .Returns(imageDataDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(imageDataDtos);
        _placeRepositoryMock.Verify(repo => repo.GetByIdAsync(query.PlaceId), Times.Once);
        _imageRepositoryMock.Verify(repo => repo.GetAllByPlaceIdAsync(query.PlaceId), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<IEnumerable<ImageDataDto>>(images), Times.Once);
    }
}
