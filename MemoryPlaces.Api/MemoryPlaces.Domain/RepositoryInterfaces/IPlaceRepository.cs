using MemoryPlaces.Domain.Entities;

namespace MemoryPlaces.Domain.RepositoryInterfaces;

public interface IPlaceRepository
{
    Task CreateAsync(Place place);
    Task<IEnumerable<Place>> GetAllAsync();
    Task<Place?> GetByIdAsync(string id);
    void Remove(Place place);
    Task Commit();
}
