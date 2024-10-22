using FluentAssertions;
using MediatR;
using MemoryPlaces.Api.Controllers;
using MemoryPlaces.Application.Place;
using MemoryPlaces.Application.Place.Commands.Create;
using MemoryPlaces.Application.Place.Commands.Delete;
using MemoryPlaces.Application.Place.Commands.Update;
using MemoryPlaces.Application.Place.Commands.Verify;
using MemoryPlaces.Application.Place.Queries.GetAll;
using MemoryPlaces.Application.Place.Queries.GetAllByUserId;
using MemoryPlaces.Application.Place.Queries.GetById;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MemoryPlaces.Api.Tests.Controllers;

public class PlaceControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly PlaceController _controller;

    public PlaceControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new PlaceController(_mediatorMock.Object);
    }

    [Fact]
    public async Task CreatePlace_ShouldReturnCreatedResult_WhenCommandIsValid()
    {
        // Arrange
        var createCommand = new CreateCommand();
        var expectedPlaceId = "new-place-id";
        _mediatorMock.Setup(m => m.Send(createCommand, default)).ReturnsAsync(expectedPlaceId);

        // Act
        var result = await _controller.CreatePlace(createCommand);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        var createdResult = result as CreatedResult;
        createdResult.Location.Should().Be(expectedPlaceId);
    }

    [Fact]
    public async Task GetAllPlaces_ShouldReturnOkWithData_WhenPlacesExist()
    {
        // Arrange
        var expectedPlaces = new List<PlaceDto>
        {
            new PlaceDto { Id = Guid.NewGuid(), Name = "Place 1" },
            new PlaceDto { Id = Guid.NewGuid(), Name = "Place 2" }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllQuery>(), default))
            .ReturnsAsync(expectedPlaces);

        // Act
        var result = await _controller.GetAllPlaces(null, null, null, null, null);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(expectedPlaces);
    }

    [Fact]
    public async Task GetPlaceById_ShouldReturnOkWithData_WhenPlaceExists()
    {
        // Arrange
        var placeId = Guid.NewGuid();
        var query = new GetByIdQuery { GivenId = placeId.ToString() };

        var expectedPlace = new PlaceDto { Id = placeId, Name = "Test Place", };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetByIdQuery>(), default))
            .ReturnsAsync(expectedPlace);

        // Act
        var result = await _controller.GetPlaceById(null, placeId.ToString());

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(expectedPlace);
    }

    [Fact]
    public async Task DeletePlaceById_ShouldReturnNoContent_WhenPlaceIsDeleted()
    {
        // Arrange
        var deleteCommand = new DeleteCommand { Id = "test-id" };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteCommand>(), default))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeletePlaceById("test-id");

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdatePlace_ShouldReturnOkWithUpdatedData_WhenCommandIsValid()
    {
        // Arrange
        var placeId = Guid.NewGuid();
        var updateCommand = new UpdateCommand { GivenId = placeId.ToString() };
        var updatedPlace = new PlaceDto { Id = placeId, Name = "Updated Place Name" };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateCommand>(), default))
            .ReturnsAsync(updatedPlace);

        // Act
        var result = await _controller.UpdatePlace(null, placeId.ToString());

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(updatedPlace);
    }

    [Fact]
    public async Task VerifyPlace_ShouldReturnNoContent_WhenCommandIsSuccessful()
    {
        // Arrange
        var verifyCommand = new VerifyCommand { GivenId = "test-id" };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<VerifyCommand>(), default))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.VerifyPlace("test-id");

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task GetPlacesByUserId_ShouldReturnOkWithData_WhenPlacesExistForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetAllByUserIdQuery { GivenUserId = userId.ToString() };

        var expectedPlaces = new List<PlaceDto>
        {
            new PlaceDto { Id = Guid.NewGuid(), Name = "Place1" },
            new PlaceDto { Id = Guid.NewGuid(), Name = "Place2" }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllByUserIdQuery>(), default))
            .ReturnsAsync(expectedPlaces);

        // Act
        var result = await _controller.GetPlacesByUserId(null, userId.ToString());

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(expectedPlaces);
    }
}
