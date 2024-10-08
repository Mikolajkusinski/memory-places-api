using MediatR;

namespace MemoryPlaces.Application.Place.Queries.GetAll;

public class GetAllQuery : IRequest<IEnumerable<PlaceDto>>
{
    public string? Locale { get; set; }
    public string? SearchPhrase { get; set; }
    public int? FilterCategoryId { get; set; }
    public int? FilterTypeId { get; set; }
    public int? FilterPeriodId { get; set; }
}
