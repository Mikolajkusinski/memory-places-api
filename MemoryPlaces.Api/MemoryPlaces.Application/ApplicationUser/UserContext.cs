using System.Security.Claims;
using MemoryPlaces.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MemoryPlaces.Application.ApplicationUser;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrnetUser? GetCurrnetUser()
    {
        var user = _httpContextAccessor?.HttpContext?.User;
        if (user == null)
        {
            throw new InvalidOperationException("Context user is not present");
        }

        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            return null;
        }

        var id = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
        var role = user.FindFirst(c => c.Type == ClaimTypes.Role)!.Value;

        return new CurrnetUser(id, email, role);
    }
}
