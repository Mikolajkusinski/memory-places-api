using MemoryPlaces.Domain.Entities;

namespace MemoryPlaces.Domain.RepositoryInterfaces;

public interface IAccountRepository
{
    Task Create(User user);
    bool AccountNameExist(string name);
    bool AccountEmailExist(string email);
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserById(string id);
    Task Commit();
}
