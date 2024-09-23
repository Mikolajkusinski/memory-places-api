using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemoryPlaces.Infrastructure.Persistance.EntitiesConfig;

public class TypeConfig : IEntityTypeConfiguration<Domain.Entities.Type>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Type> builder)
    {
        builder.Property(r => r.PolishName).IsRequired();
        builder.Property(r => r.EnglishName).IsRequired();
        builder.Property(r => r.GermanName).IsRequired();
        builder.Property(r => r.RussianName).IsRequired();
    }
}
