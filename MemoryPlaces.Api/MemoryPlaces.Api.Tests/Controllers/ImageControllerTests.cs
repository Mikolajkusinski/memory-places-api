using FluentAssertions;
using MediatR;
using MemoryPlaces.Api.Controllers;
using MemoryPlaces.Application.Image;
using MemoryPlaces.Application.Image.Commands.DeleteImage;
using MemoryPlaces.Application.Image.Commands.UploadImages;
using MemoryPlaces.Application.Image.Queries.GetAllImages;
using MemoryPlaces.Application.Image.Queries.GetAllImagesByPlaceId;
using MemoryPlaces.Application.Image.Queries.GetImageById;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MemoryPlaces.Api.Tests.Controllers;

public class ImageControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ImageController _controller;

    public ImageControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ImageController(_mediatorMock.Object);
    }

    private IFormFile CreateMockFormFile(
        long size,
        string contentType = "image/png",
        string fileName = "test.png"
    )
    {
        var stream = new MemoryStream(new byte[size]);
        return new FormFile(stream, 0, size, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }

    [Fact]
    public async Task UploadImages_ShouldReturnBadRequest_WhenMoreThanThreeFilesUploaded()
    {
        // Arrange
        var files = new List<IFormFile>
        {
            CreateMockFormFile(1 * 1024 * 1024),
            CreateMockFormFile(1 * 1024 * 1024),
            CreateMockFormFile(1 * 1024 * 1024),
            CreateMockFormFile(1 * 1024 * 1024)
        };

        // Act
        var result = await _controller.UploadImages(files, "placeId");

        // Assert
        result
            .Should()
            .BeOfType<BadRequestObjectResult>()
            .Which.Value.Should()
            .Be("You can upload a maximum of 3 files.");
    }

    [Fact]
    public async Task UploadImages_ShouldReturnBadRequest_WhenFileExceedsMaxSize()
    {
        // Arrange
        var files = new List<IFormFile> { CreateMockFormFile(3 * 1024 * 1024) };

        // Act
        var result = await _controller.UploadImages(files, "placeId");

        // Assert
        result
            .Should()
            .BeOfType<BadRequestObjectResult>()
            .Which.Value.Should()
            .Be("File 'test.png' exceeds the 2 MB size limit.");
    }

    [Fact]
    public async Task UploadImages_ShouldReturnOk_WhenFilesAreValid()
    {
        // Arrange
        var files = new List<IFormFile> { CreateMockFormFile(1 * 1024 * 1024) };
        var mockBlobUris = new List<string> { "https://example.com/image1.png" };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UploadImagesCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockBlobUris);

        // Act
        var result = await _controller.UploadImages(files, "placeId");

        // Assert
        result
            .Should()
            .BeOfType<OkObjectResult>()
            .Which.Value.Should()
            .BeEquivalentTo(mockBlobUris);
    }

    [Fact]
    public async Task DeleteImage_ShouldReturnNoContent()
    {
        // Arrange
        var imageId = 1;
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteImageCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteImage(imageId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task GetAllImages_ShouldReturnOkWithData()
    {
        // Arrange
        var images = new List<ImageDataDto>
        {
            new ImageDataDto
            {
                Id = 1,
                ImagePath = "test",
                PlaceId = "test"
            }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllImagesQuery>(), default))
            .ReturnsAsync(images);

        // Act
        var result = await _controller.GetAllImages();

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(images);
    }

    [Fact]
    public async Task GetAllImagesByPlaceId_ShouldReturnOkWithData()
    {
        // Arrange
        var placeId = "test";
        var images = new List<ImageDataDto>
        {
            new ImageDataDto
            {
                Id = 1,
                ImagePath = "test",
                PlaceId = "test"
            }
        };

        _mediatorMock
            .Setup(m =>
                m.Send(
                    It.Is<GetAllImagesByPlaceIdQuery>(q => q.PlaceId == placeId),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(images);

        // Act
        var result = await _controller.GetAllImagesByPlaceId(placeId);

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(images);
    }

    [Fact]
    public async Task GetImageById_ShouldReturnOkWithData()
    {
        // Arrange
        var imageId = 1;
        var image = new ImageDataDto
        {
            Id = 1,
            ImagePath = "test",
            PlaceId = "test"
        };

        _mediatorMock
            .Setup(m =>
                m.Send(
                    It.Is<GetImageByIdQuery>(q => q.ImageId == imageId),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(image);

        // Act
        var result = await _controller.GetImageById(imageId);

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(image);
    }
}
