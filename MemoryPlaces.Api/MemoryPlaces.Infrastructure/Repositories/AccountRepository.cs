using MemoryPlaces.Domain.Entities;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace MemoryPlaces.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly MemoryPlacesDbContext _dbContext;

    public AccountRepository(MemoryPlacesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool AccountEmailExist(string email)
    {
        var emailInUse = _dbContext.Users.Any(u => u.Email == email);
        return emailInUse;
    }

    public bool AccountNameExist(string name)
    {
        var nameInUse = _dbContext.Users.Any(u => u.Name == name);
        return nameInUse;
    }

    public async Task Create(User user)
    {
        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await _dbContext
            .Users.Include(r => r.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }
}
