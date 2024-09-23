using MemoryPlaces.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MemoryPlaces.Infrastructure.Persistance.EntitiesConfig;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Name).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(50);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.IsActive).HasDefaultValue(false);
        builder.Property(u => u.IsConfirmed).HasDefaultValue(false);
        builder.Property(u => u.Created).IsRequired();

        builder.HasMany(u => u.Places).WithOne(p => p.Author).HasForeignKey(p => p.AuthorId);
        builder.HasOne(u => u.Role).WithMany().HasForeignKey(u => u.RoleId);
    }
}
