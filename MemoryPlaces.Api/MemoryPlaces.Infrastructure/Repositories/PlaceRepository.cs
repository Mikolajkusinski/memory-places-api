using MemoryPlaces.Domain.Entities;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace MemoryPlaces.Infrastructure.Repositories;

public class PlaceRepository : IPlaceRepository
{
    private readonly MemoryPlacesDbContext _dbContext;

    public PlaceRepository(MemoryPlacesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(Place place)
    {
        _dbContext.Add(place);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Place>> GetAllAsync() =>
        await _dbContext
            .Places.Include(x => x.Type)
            .Include(x => x.Period)
            .Include(x => x.Category)
            .Include(x => x.Author)
            .ToListAsync();
}
