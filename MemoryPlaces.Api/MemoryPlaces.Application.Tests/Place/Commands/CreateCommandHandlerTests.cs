using AutoMapper;
using FluentAssertions;
using MemoryPlaces.Application.Place.Commands.Create;
using MemoryPlaces.Domain.RepositoryInterfaces;
using Moq;

namespace MemoryPlaces.Application.Tests.Place.Commands;

public class CreateCommandHandlerTests
{
    private readonly Mock<IPlaceRepository> _placeRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateCommandHandler _handler;

    public CreateCommandHandlerTests()
    {
        _placeRepositoryMock = new Mock<IPlaceRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CreateCommandHandler(_placeRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallCreateAsyncOnRepository()
    {
        // Arrange
        var createCommand = new CreateCommand
        {
            Name = "Test Place",
            Description = "Test Description",
            Latitude = 51.5074m,
            Longitude = -0.1278m,
            AuthorId = Guid.NewGuid(),
            TypeId = 1,
            PeriodId = 1,
            CategoryId = 1
        };
        var place = new Domain.Entities.Place { Id = Guid.NewGuid() };

        _mapperMock.Setup(m => m.Map<Domain.Entities.Place>(createCommand)).Returns(place);

        // Act
        await _handler.Handle(createCommand, CancellationToken.None);

        // Assert
        _placeRepositoryMock.Verify(r => r.CreateAsync(place), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnPlaceIdAsString()
    {
        // Arrange
        var createCommand = new CreateCommand
        {
            Name = "Test Place",
            Description = "Test Description",
            Latitude = 51.5074m,
            Longitude = -0.1278m,
            AuthorId = Guid.NewGuid(),
            TypeId = 1,
            PeriodId = 1,
            CategoryId = 1
        };
        var place = new Domain.Entities.Place { Id = Guid.NewGuid() };

        _mapperMock.Setup(m => m.Map<Domain.Entities.Place>(createCommand)).Returns(place);

        // Act
        var result = await _handler.Handle(createCommand, CancellationToken.None);

        // Assert
        result.Should().Be(place.Id.ToString());
    }
}
