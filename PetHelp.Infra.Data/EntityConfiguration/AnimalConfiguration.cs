using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHelp.Domain.Entities;

namespace PetHelp.Infra.Data.EntityConfiguration;

public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
{
    public void Configure(EntityTypeBuilder<Animal> entity)
    {
        entity.ToTable("Animals");

        entity.HasKey(a => a.Id);

        entity.Property(a => a.Name).IsRequired().HasMaxLength(100);
        entity.Property(a => a.Species).IsRequired().HasMaxLength(50);
        entity.Property(a => a.Breed).IsRequired().HasMaxLength(50);

        entity.Property(a => a.BirthDate)
              .IsRequired();

        entity.Property(a => a.Description).IsRequired().HasMaxLength(500);
        entity.Property(a => a.ImageUrl).HasMaxLength(200);
        entity.Property(a => a.City).IsRequired().HasMaxLength(100);
        entity.Property(a => a.State).IsRequired().HasMaxLength(50);
        entity.Property(a => a.CreatedByUserId).IsRequired();

        entity.Property(a => a.Gender)
              .HasConversion<string>()
              .IsRequired();

        entity.Property(a => a.Size)
              .HasConversion<string>()
              .IsRequired();

        entity.Property(a => a.Status)
              .HasConversion<string>()
              .IsRequired();

        entity.Property(a => a.PhotoUrls)
              .HasConversion(
                  v => string.Join(";", v),
                  v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
              )
              .HasColumnName("PhotoUrls")
              .HasMaxLength(1000);
    }
}
