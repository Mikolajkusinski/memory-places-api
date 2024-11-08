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

    public async Task Commit()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateAsync(Place place)
    {
        _dbContext.Add(place);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Place>> GetAllAsync(
        string? searchPhrase,
        int? filterCategoryId,
        int? filterTypeId,
        int? filterPeriodId
    )
    {
        var query = _dbContext
            .Places.Include(x => x.Type)
            .Include(x => x.Period)
            .Include(x => x.Category)
            .Include(x => x.Author)
            .Where(x => searchPhrase == null || x.Name.ToLower().Contains(searchPhrase.ToLower()))
            .AsQueryable();

        if (filterCategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == filterCategoryId);
        }

        if (filterTypeId.HasValue)
        {
            query = query.Where(p => p.TypeId == filterTypeId);
        }

        if (filterPeriodId.HasValue)
        {
            query = query.Where(p => p.PeriodId == filterPeriodId);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Place>> GetAllByUserIdAsync(string userId) =>
        await _dbContext
            .Places.Where(x => x.AuthorId.ToString() == userId)
            .Include(x => x.Type)
            .Include(x => x.Period)
            .Include(x => x.Category)
            .Include(x => x.Author)
            .ToListAsync();

    public async Task<Place?> GetByIdAsync(string id) =>
        await _dbContext
            .Places.Include(x => x.Type)
            .Include(x => x.Period)
            .Include(x => x.Category)
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id.ToString() == id);

    public void Remove(Place place)
    {
        _dbContext.Places.Remove(place);
    }
}
