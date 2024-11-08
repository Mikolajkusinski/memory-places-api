using FluentAssertions;
using MemoryPlaces.Application.ApplicationUser;

namespace MemoryPlaces.Application.Tests.ApplicationUser;

public class ApplicationUserTests
{
    [Fact()]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue()
    {
        // arrange

        var currentUser = new CurrentUser("1", "test@tests.com", "Admin");

        // act

        var isInRole = currentUser.IsInRole("Admin");

        // assert

        isInRole.Should().BeTrue();
    }

    [Fact()]
    public void IsInRole_WithNonMatchingRole_ShouldReturnFalse()
    {
        // arrange

        var currentUser = new CurrentUser("1", "test@tests.com", "Admin");

        // act

        var isInRole = currentUser.IsInRole("Manager");

        // assert

        isInRole.Should().BeFalse();
    }

    [Fact()]
    public void IsInRole_WithNonMatchingCaseRole_ShouldReturnFalse()
    {
        // arrange

        var currentUser = new CurrentUser("1", "test@tests.com", "Admin");

        // act

        var isInRole = currentUser.IsInRole("admin");

        // assert

        isInRole.Should().BeFalse();
    }
}
