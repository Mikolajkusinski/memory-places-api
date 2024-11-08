using MemoryPlaces.Domain.Entities;

namespace MemoryPlaces.Domain.RepositoryInterfaces;

public interface IPlaceRepository
{
    Task CreateAsync(Place place);
    Task<IEnumerable<Place>> GetAllAsync(
        string? searchPhrase,
        int? categoryId,
        int? typeId,
        int? periodId
    );
    Task<Place?> GetByIdAsync(string id);
    Task<IEnumerable<Place>> GetAllByUserIdAsync(string userId);
    void Remove(Place place);
    Task Commit();
}
