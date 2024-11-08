using AutoMapper;
using FluentAssertions;
using MemoryPlaces.Application.Account;
using MemoryPlaces.Application.Mappings;
using MemoryPlaces.Application.Place;
using MemoryPlaces.Domain.Entities;

namespace MemoryPlaces.Application.Tests.Mappings;

public class MemoryPlacesMappingProfileTests
{
    private readonly IMapper _mapper;

    public MemoryPlacesMappingProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MemoryPlacesMappingProfile>();
        });

        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void MemoryPlacesMappingProfile_ShouldMapRegisterDtoToUser()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            ConfirmPassword = "password123",
            IsActive = true,
            IsConfirmed = true,
            RoleId = 2
        };

        // Act
        var user = _mapper.Map<User>(registerDto);

        // Assert
        user.Should().NotBeNull();
        user.Name.Should().Be(registerDto.Name);
        user.Email.Should().Be(registerDto.Email);
        user.IsActive.Should().Be(registerDto.IsActive);
        user.IsConfirmed.Should().Be(registerDto.IsConfirmed);
        user.RoleId.Should().Be(registerDto.RoleId);
    }

    [Fact]
    public void CreatePlaceDto_To_Place_Should_Map_Correctly()
    {
        // Arrange
        var createPlaceDto = new CreatePlaceDto
        {
            Name = "Test Place",
            Description = "A great test place.",
            Latitude = 51.5074m,
            Longitude = 0.1278m,
            WikipediaLink = "https://en.wikipedia.org/wiki/Test_Place",
            WebsiteLink = "https://testplace.com",
            AuthorId = Guid.NewGuid(),
            TypeId = 1,
            PeriodId = 2,
            CategoryId = 3
        };

        // Act
        var placeEntity = _mapper.Map<Domain.Entities.Place>(createPlaceDto);

        // Assert
        placeEntity.Should().NotBeNull();
        placeEntity.Name.Should().Be(createPlaceDto.Name);
        placeEntity.Description.Should().Be(createPlaceDto.Description);
        placeEntity.Latitude.Should().Be(createPlaceDto.Latitude);
        placeEntity.Longitude.Should().Be(createPlaceDto.Longitude);
        placeEntity.WikipediaLink.Should().Be(createPlaceDto.WikipediaLink);
        placeEntity.WebsiteLink.Should().Be(createPlaceDto.WebsiteLink);
        placeEntity.AuthorId.Should().Be(createPlaceDto.AuthorId);
        placeEntity.TypeId.Should().Be(createPlaceDto.TypeId);
        placeEntity.PeriodId.Should().Be(createPlaceDto.PeriodId);
        placeEntity.CategoryId.Should().Be(createPlaceDto.CategoryId);
    }

    [Fact]
    public void Place_To_PlaceDto_Should_Map_Correctly()
    {
        // Arrange
        var place = new Domain.Entities.Place
        {
            Id = Guid.NewGuid(),
            Name = "Test Place",
            Description = "A beautiful test place",
            Latitude = 51.5074m,
            Longitude = 0.1278m,
            WikipediaLink = "https://en.wikipedia.org/wiki/Test_Place",
            WebsiteLink = "https://testplace.com",
            Created = DateTime.UtcNow,
            IsVerified = true,
            VerificationDate = DateTime.UtcNow,
            Author = new User { Id = Guid.NewGuid(), Name = "John Doe" },
            Type = new Domain.Entities.Type { Id = 1, PolishName = "Miejsce historyczne" },
            Period = new Period { Id = 1, PolishName = "XIX wiek" },
            Category = new Category { Id = 1, PolishName = "Muzeum" }
        };
        var locale = "pl";

        // Act
        var placeDto = _mapper.Map<PlaceDto>(place, opts => opts.Items["Locale"] = locale);

        // Assert
        placeDto.Should().NotBeNull();
        placeDto.Id.Should().Be(place.Id);
        placeDto.Name.Should().Be(place.Name);
        placeDto.Description.Should().Be(place.Description);
        placeDto.Latitude.Should().Be(place.Latitude);
        placeDto.Longitude.Should().Be(place.Longitude);
        placeDto.WikipediaLink.Should().Be(place.WikipediaLink);
        placeDto.WebsiteLink.Should().Be(place.WebsiteLink);
        placeDto.Created.Should().Be(place.Created);
        placeDto.IsVerified.Should().Be(place.IsVerified);
        placeDto.VerificationDate.Should().Be(place.VerificationDate);

        placeDto.AuthorId.Should().Be(place.Author.Id);
        placeDto.AuthorName.Should().Be(place.Author.Name);
        placeDto.TypeId.Should().Be(place.Type.Id);
        placeDto.TypeName.Should().Be(place.Type.GetLocalizedName("pl"));
        placeDto.PeriodId.Should().Be(place.Period.Id);
        placeDto.PeriodName.Should().Be(place.Period.GetLocalizedName("pl"));
        placeDto.CategoryId.Should().Be(place.Category.Id);
        placeDto.CategoryName.Should().Be(place.Category.GetLocalizedName("pl"));
    }
}
