using AutoMapper;
using FluentAssertions;
using MemoryPlaces.Application.Place;
using MemoryPlaces.Application.Place.Queries.GetAll;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Shared.Exceptions;
using Moq;

namespace MemoryPlaces.Application.Tests.Place.Queries;

public class GetAllQueryHandlerTests
{
    private readonly Mock<IPlaceRepository> _placeRepositoryMock;
    private readonly IMapper _mapper;
    private readonly GetAllQueryHandler _handler;

    public GetAllQueryHandlerTests()
    {
        _placeRepositoryMock = new Mock<IPlaceRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Domain.Entities.Place, PlaceDto>();
        });

        _mapper = config.CreateMapper();

        _handler = new GetAllQueryHandler(_placeRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsPlaceDtos()
    {
        // Arrange
        var request = new GetAllQuery
        {
            SearchPhrase = "Park",
            FilterCategoryId = null,
            FilterTypeId = null,
            FilterPeriodId = null,
            Locale = "en"
        };

        var places = new List<Domain.Entities.Place>
        {
            new() { Id = Guid.NewGuid(), Name = "Central Park" },
            new() { Id = Guid.NewGuid(), Name = "Golden Gate Park" }
        };

        _placeRepositoryMock
            .Setup(repo =>
                repo.GetAllAsync(
                    request.SearchPhrase,
                    request.FilterCategoryId,
                    request.FilterTypeId,
                    request.FilterPeriodId
                )
            )
            .ReturnsAsync(places);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeOfType<List<PlaceDto>>();
        result.First().Name.Should().Be("Central Park");
    }

    [Fact]
    public async Task Handle_RepositoryReturnsEmptyList_ReturnsEmptyList()
    {
        // Arrange
        var request = new GetAllQuery
        {
            SearchPhrase = "Nonexistent Place",
            FilterCategoryId = 1,
            FilterTypeId = 2,
            FilterPeriodId = 3,
            Locale = "en"
        };

        _placeRepositoryMock
            .Setup(repo =>
                repo.GetAllAsync(
                    request.SearchPhrase,
                    request.FilterCategoryId,
                    request.FilterTypeId,
                    request.FilterPeriodId
                )
            )
            .ReturnsAsync(new List<Domain.Entities.Place>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_NullLocale_UsesDefaultLocale()
    {
        // Arrange
        var request = new GetAllQuery
        {
            SearchPhrase = "Park",
            FilterCategoryId = null,
            FilterTypeId = null,
            FilterPeriodId = null,
            Locale = null
        };

        var places = new List<Domain.Entities.Place>
        {
            new() { Id = Guid.NewGuid(), Name = "Central Park" }
        };

        _placeRepositoryMock
            .Setup(repo =>
                repo.GetAllAsync(
                    request.SearchPhrase,
                    request.FilterCategoryId,
                    request.FilterTypeId,
                    request.FilterPeriodId
                )
            )
            .ReturnsAsync(places);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Central Park");
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_ThrowsSameException()
    {
        // Arrange
        var request = new GetAllQuery
        {
            SearchPhrase = "Park",
            FilterCategoryId = 1,
            FilterTypeId = 2,
            FilterPeriodId = 3,
            Locale = "en"
        };

        _placeRepositoryMock
            .Setup(repo =>
                repo.GetAllAsync(
                    request.SearchPhrase,
                    request.FilterCategoryId,
                    request.FilterTypeId,
                    request.FilterPeriodId
                )
            )
            .ThrowsAsync(new NotFoundException("Place not found"));

        // Act & Assert
        var act = async () => await _handler.Handle(request, CancellationToken.None);
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Place not found");
    }
}
