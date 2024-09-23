using MemoryPlaces.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemoryPlaces.Infrastructure.Persistance.EntitiesConfig;

public class PeriodConfig : IEntityTypeConfiguration<Period>
{
    public void Configure(EntityTypeBuilder<Period> builder)
    {
        builder.Property(r => r.PolishName).IsRequired();
        builder.Property(r => r.EnglishName).IsRequired();
        builder.Property(r => r.GermanName).IsRequired();
        builder.Property(r => r.RussianName).IsRequired();
    }
}
