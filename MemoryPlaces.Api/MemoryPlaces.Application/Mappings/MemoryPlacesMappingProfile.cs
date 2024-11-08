using AutoMapper;
using MemoryPlaces.Application.Account;
using MemoryPlaces.Application.Image;
using MemoryPlaces.Application.Place;
using MemoryPlaces.Domain.Entities;

namespace MemoryPlaces.Application.Mappings;

public class MemoryPlacesMappingProfile : Profile
{
    public MemoryPlacesMappingProfile()
    {
        CreateMap<RegisterDto, User>();

        CreateMap<CreatePlaceDto, Domain.Entities.Place>();

        CreateMap<Domain.Entities.Place, PlaceDto>()
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Author.Id))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
            .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.Type.Id))
            .ForMember(
                dest => dest.TypeName,
                opt =>
                    opt.MapFrom(
                        (src, dest, _, context) =>
                        {
                            var locale = context.Items["Locale"] as string;
                            return src.Type.GetLocalizedName(locale ?? "en");
                        }
                    )
            )
            .ForMember(dest => dest.PeriodId, opt => opt.MapFrom(src => src.Period.Id))
            .ForMember(
                dest => dest.PeriodName,
                opt =>
                    opt.MapFrom(
                        (src, dest, _, context) =>
                        {
                            var locale = context.Items["Locale"] as string;
                            return src.Period.GetLocalizedName(locale ?? "en");
                        }
                    )
            )
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id))
            .ForMember(
                dest => dest.CategoryName,
                opt =>
                    opt.MapFrom(
                        (src, dest, _, context) =>
                        {
                            var locale = context.Items["Locale"] as string;
                            return src.Category.GetLocalizedName(locale ?? "en");
                        }
                    )
            );

        CreateMap<Domain.Entities.Image, ImageDataDto>().ReverseMap();
    }
}
