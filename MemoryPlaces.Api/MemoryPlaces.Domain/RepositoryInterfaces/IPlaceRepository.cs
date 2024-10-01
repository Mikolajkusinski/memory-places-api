using MemoryPlaces.Domain.Entities;

namespace MemoryPlaces.Domain.RepositoryInterfaces;

public interface IPlaceRepository
{
    Task CreateAsync(Place place);
    Task<IEnumerable<Domain.Entities.Place>> GetAllAsync();
}
