using AutoMapper;
using MemoryPlaces.Application.Account;
using MemoryPlaces.Application.Place;
using MemoryPlaces.Domain.Entities;

namespace MemoryPlaces.Application.Mappings;

public class MemoryPlacesMappingProfile : Profile
{
    public MemoryPlacesMappingProfile()
    {
        CreateMap<RegisterDto, User>();

        CreateMap<CreatePlaceDto, Domain.Entities.Place>();
    }
}
