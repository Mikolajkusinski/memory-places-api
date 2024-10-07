using MemoryPlaces.Application.ApplicationUser;

namespace MemoryPlaces.Application.Interfaces;

public interface IUserContext
{
    CurrnetUser? GetCurrnetUser();
}
