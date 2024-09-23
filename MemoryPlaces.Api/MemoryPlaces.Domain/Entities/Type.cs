namespace MemoryPlaces.Domain.Entities;

public class Type
{
    public int Id { get; set; }
    public string PolishName { get; set; } = default!;
    public string EnglishName { get; set; } = default!;
    public string GermanName { get; set; } = default!;
    public string RussianName { get; set; } = default!;
}
