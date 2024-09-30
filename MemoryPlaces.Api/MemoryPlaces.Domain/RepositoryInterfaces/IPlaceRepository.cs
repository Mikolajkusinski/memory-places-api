using MemoryPlaces.Domain.Entities;

namespace MemoryPlaces.Domain.RepositoryInterfaces;

public interface IPlaceRepository
{
    Task Create(Place place);
}
