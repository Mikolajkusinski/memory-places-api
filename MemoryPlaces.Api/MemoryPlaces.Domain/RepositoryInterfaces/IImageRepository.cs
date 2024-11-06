using MemoryPlaces.Domain.Entities;

namespace MemoryPlaces.Domain.RepositoryInterfaces;

public interface IImageRepository
{
    void Add(Image image);
    Task<Image?> GetByIdAsync(int id);
    Task<IEnumerable<Image>> GetAllAsync();
    Task<IEnumerable<Image>> GetAllByPlaceIdAsync(string placeId);
    void Remove(Image image);
    Task Commit();
}
