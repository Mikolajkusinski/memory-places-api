namespace MemoryPlaces.Domain.Entities;

public abstract class LocalizableEntity
{
    public string PolishName { get; set; } = default!;
    public string EnglishName { get; set; } = default!;
    public string GermanName { get; set; } = default!;
    public string RussianName { get; set; } = default!;

    public string GetLocalizedName(string locale)
    {
        return locale switch
        {
            "pl" => PolishName,
            "en" => EnglishName,
            "de" => GermanName,
            "ru" => RussianName,
            _ => EnglishName
        };
    }
}
