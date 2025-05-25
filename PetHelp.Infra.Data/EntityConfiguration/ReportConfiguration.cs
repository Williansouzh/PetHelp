using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHelp.Domain.Entities;

namespace PetHelp.Infra.Data.EntityConfiguration;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> entity)
    {
        entity.ToTable("Reports");

        entity.HasKey(r => r.Id);

        entity.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(1000);

        entity.Property(r => r.ImageUrl)
            .HasMaxLength(500)
            .IsRequired(false);

        entity.Property(r => r.Latitude)
            .IsRequired();

        entity.Property(r => r.Longitude)
            .IsRequired();

        entity.Property(r => r.Address)
            .IsRequired()
            .HasMaxLength(500);

        entity.Property(r => r.AnimalType)
            .IsRequired();

        entity.Property(r => r.UrgencyLevel)
            .IsRequired();

        entity.Property(r => r.UserId)
            .IsRequired(false);

        entity.Property(r => r.Name)
            .IsRequired(false)
            .HasMaxLength(100);

        entity.Property(r => r.Phone)
            .IsRequired(false)
            .HasMaxLength(20);

        entity.Property(r => r.Email)
            .IsRequired(false)
            .HasMaxLength(100);
    }
}
