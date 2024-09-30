using MemoryPlaces.Domain.Entities;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Infrastructure.Persistance;

namespace MemoryPlaces.Infrastructure.Repositories;

public class PlaceRepository : IPlaceRepository
{
    private readonly MemoryPlacesDbContext _dbContext;

    public PlaceRepository(MemoryPlacesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Create(Place place)
    {
        _dbContext.Add(place);
        await _dbContext.SaveChangesAsync();
    }
}
