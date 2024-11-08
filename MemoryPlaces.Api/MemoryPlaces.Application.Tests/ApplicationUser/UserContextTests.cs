using System.Security.Claims;
using FluentAssertions;
using MemoryPlaces.Application.ApplicationUser;
using Microsoft.AspNetCore.Http;
using Moq;

namespace MemoryPlaces.Application.Tests.ApplicationUser;

public class UserContextTests
{
    [Fact()]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        // arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Role, "User")
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        httpContextAccessorMock
            .Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext() { User = user });

        var userContext = new UserContext(httpContextAccessorMock.Object);
        // act

        var currentUser = userContext.GetCurrentUser();

        // arrange

        currentUser.Should().NotBeNull();
        currentUser!.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@example.com");
        currentUser.Role.Should().Contain("User");
    }
}
