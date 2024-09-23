using MemoryPlaces.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemoryPlaces.Infrastructure.Persistance.EntitiesConfig;

public class PlaceConfig : IEntityTypeConfiguration<Place>
{
    public void Configure(EntityTypeBuilder<Place> builder)
    {
        builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(500);
        builder.Property(p => p.Latitude).IsRequired().HasPrecision(9, 6);
        builder.Property(p => p.Longitude).IsRequired().HasPrecision(9, 6);
        builder.Property(p => p.Created).IsRequired();
        builder.Property(p => p.IsVerified).HasDefaultValue(false);

        builder.HasOne(p => p.Type).WithMany().HasForeignKey(p => p.TypeId);
        builder.HasOne(p => p.Period).WithMany().HasForeignKey(p => p.PeriodId);
        builder.HasOne(p => p.Category).WithMany().HasForeignKey(p => p.CategoryId);
    }
}
