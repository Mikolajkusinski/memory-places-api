using AutoMapper;
using FluentAssertions;
using MemoryPlaces.Application.Account;
using MemoryPlaces.Application.Mappings;
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
}
