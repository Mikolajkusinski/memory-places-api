using AutoMapper;
using FluentAssertions;
using MemoryPlaces.Application.Interfaces;
using MemoryPlaces.Application.Place;
using MemoryPlaces.Application.Place.Queries.GetAllByUserId;
using MemoryPlaces.Domain.RepositoryInterfaces;
using Moq;

namespace MemoryPlaces.Application.Tests.Place.Queries;

public class GetByUserIdQueryHandlerTests
{
    private readonly Mock<IPlaceRepository> _placeRepositoryMock;
    private readonly IMapper _mapper;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly GetByUserIdQueryHandler _handler;

    public GetByUserIdQueryHandlerTests()
    {
        _placeRepositoryMock = new Mock<IPlaceRepository>();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Domain.Entities.Place, PlaceDto>();
        });

        _mapper = config.CreateMapper();
        _userContextMock = new Mock<IUserContext>();
        _handler = new GetByUserIdQueryHandler(
            _placeRepositoryMock.Object,
            _mapper,
            _userContextMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedPlaces_ForGivenUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var places = new List<Domain.Entities.Place>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Test Place",
                Created = DateTime.MinValue
            }
        };
        var expectedPlaceDtos = new List<PlaceDto>
        {
            new() { Id = places[0].Id, Name = "Test Place" }
        };

        _placeRepositoryMock
            .Setup(repo => repo.GetAllByUserIdAsync(userId.ToString()))
            .ReturnsAsync(places);

        var query = new GetAllByUserIdQuery { GivenUserId = userId.ToString(), Locale = "en" };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedPlaceDtos);
        _placeRepositoryMock.Verify(
            repo => repo.GetAllByUserIdAsync(userId.ToString()),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_ShouldUseDefaultLocale_WhenLocaleIsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var places = new List<Domain.Entities.Place>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Test Place",
                Created = DateTime.MinValue
            }
        };
        var expectedPlaceDtos = new List<PlaceDto>
        {
            new() { Id = places[0].Id, Name = "Test Place", }
        };

        _placeRepositoryMock
            .Setup(repo => repo.GetAllByUserIdAsync(userId.ToString()))
            .ReturnsAsync(places);

        var query = new GetAllByUserIdQuery { GivenUserId = userId.ToString(), Locale = null };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedPlaceDtos);
        _placeRepositoryMock.Verify(
            repo => repo.GetAllByUserIdAsync(userId.ToString()),
            Times.Once
        );
    }
}
