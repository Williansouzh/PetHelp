using Microsoft.EntityFrameworkCore;
using PetHelp.Domain.Entities;

namespace PetHelp.Infra.Data.EntityConfiguration;

public class AnimalConfiguration
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>(entity =>
        {
            entity.ToTable("Animals");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Name).IsRequired().HasMaxLength(100);
            entity.Property(a => a.Species).IsRequired().HasMaxLength(50);
            entity.Property(a => a.Breed).IsRequired().HasMaxLength(50);
            entity.Property(a => a.Age).IsRequired();
            entity.Property(a => a.Description).IsRequired().HasMaxLength(500);
            entity.Property(a => a.ImageUrl).HasMaxLength(200);
            entity.Property(a => a.City).IsRequired().HasMaxLength(100);
            entity.Property(a => a.State).IsRequired().HasMaxLength(50);
        });
    }
}
