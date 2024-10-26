using AutoMapper;
using FluentAssertions;
using MemoryPlaces.Application.Place;
using MemoryPlaces.Application.Place.Queries.GetById;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;
using Moq;

namespace MemoryPlaces.Application.Tests.Place.Queries;

public class GetByIdQueryHandlerTests
{
    private readonly Mock<IPlaceRepository> _placeRepositoryMock;
    private readonly IMapper _mapper;

    private readonly GetByIdQueryHandler _handler;

    public GetByIdQueryHandlerTests()
    {
        _placeRepositoryMock = new Mock<IPlaceRepository>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Domain.Entities.Place, PlaceDto>();
        });

        _mapper = config.CreateMapper();
        _handler = new GetByIdQueryHandler(_placeRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnPlaceDto_WhenPlaceExists()
    {
        // Arrange
        var placeId = Guid.NewGuid();
        var place = new Domain.Entities.Place { Id = placeId, Name = "Test Place", };
        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(placeId.ToString()))
            .ReturnsAsync(place);

        var query = new GetByIdQuery { GivenId = placeId.ToString(), Locale = "en" };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(placeId);
        result.Name.Should().Be("Test Place");
        result.Created.Should().Be(place.Created);
        _placeRepositoryMock.Verify(repo => repo.GetByIdAsync(placeId.ToString()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenPlaceDoesNotExist()
    {
        // Arrange
        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Domain.Entities.Place)null);

        var query = new GetByIdQuery { GivenId = Guid.NewGuid().ToString() };

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Place not found");
        _placeRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldUseDefaultLocale_WhenLocaleIsNull()
    {
        // Arrange
        var placeId = Guid.NewGuid();
        var place = new Domain.Entities.Place { Id = placeId, Name = "Test Place", };
        _placeRepositoryMock
            .Setup(repo => repo.GetByIdAsync(placeId.ToString()))
            .ReturnsAsync(place);

        var query = new GetByIdQuery { GivenId = placeId.ToString(), Locale = null };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(placeId);
        result.Name.Should().Be("Test Place");
        result.Created.Should().Be(place.Created);
        _placeRepositoryMock.Verify(repo => repo.GetByIdAsync(placeId.ToString()), Times.Once);
    }
}
