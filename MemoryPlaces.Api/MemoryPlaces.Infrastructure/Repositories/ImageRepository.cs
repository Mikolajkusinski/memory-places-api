using MemoryPlaces.Domain.Entities;
using MemoryPlaces.Domain.RepositoryInterfaces;
using MemoryPlaces.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace MemoryPlaces.Infrastructure.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly MemoryPlacesDbContext _dbContext;

    public ImageRepository(MemoryPlacesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Image image)
    {
        _dbContext.Images.Add(image);
    }

    public async Task Commit()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Image>> GetAllAsync() => await _dbContext.Images.ToListAsync();

    public async Task<IEnumerable<Image>> GetAllByPlaceIdAsync(string placeId) =>
        await _dbContext.Images.Where(x => x.PlaceId.ToString() == placeId).ToListAsync();

    public async Task<Image?> GetByIdAsync(int id) =>
        await _dbContext.Images.FirstOrDefaultAsync(x => x.Id == id);

    public void Remove(Image image)
    {
        _dbContext.Images.Remove(image);
    }
}
