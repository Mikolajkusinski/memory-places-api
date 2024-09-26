using AutoMapper;
using MemoryPlaces.Application.Account;
using MemoryPlaces.Domain.Entities;

namespace MemoryPlaces.Application.Mappings;

public class MemoryPlacesMappingProfile : Profile
{
    public MemoryPlacesMappingProfile()
    {
        CreateMap<RegisterDto, User>();
    }
}
