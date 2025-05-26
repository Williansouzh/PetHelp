using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetHelp.Domain.Entities;

namespace PetHelp.Infra.Data.EntityConfiguration;

public class AdoptionConfiguration : IEntityTypeConfiguration<Adoption>
{
    public void Configure(EntityTypeBuilder<Adoption> builder)
    {
        builder.ToTable("Adoptions");  // Nome da tabela

        builder.HasKey(a => a.Id);

        builder.Property(a => a.AnimalId)
            .IsRequired();

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasMaxLength(450); // Ajuste conforme o tamanho do seu UserId

        builder.Property(a => a.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.HasOtherPets)
            .IsRequired();

        builder.Property(a => a.HousingType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.NumberOfResidents)
            .IsRequired();

        builder.Property(a => a.WorkSchedule)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.ReasonForAdoption)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.AgreedToTerms)
            .IsRequired();

        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion<int>();  // Se for enum

        builder.Property(a => a.CreatedAt)
            .IsRequired();
    }

}