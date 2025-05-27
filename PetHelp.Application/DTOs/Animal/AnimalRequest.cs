using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using static PetHelp.Domain.Enum.AnimalEnums;

namespace PetHelp.Application.DTOs.Animal;

public class AnimalRequest
{
    public Guid? Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(50)]
    public string Species { get; set; }

    [Required]
    [StringLength(50)]
    public string Breed { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [Required]
    public Size Size { get; set; }

    [Required]
    [StringLength(1000)]
    public string Description { get; set; }

    public bool IsVaccinated { get; set; }

    public bool IsNeutered { get; set; }

    [StringLength(1000)]
    public string AdoptionRequirements { get; set; }

    public AnimalStatus Status { get; set; } = AnimalStatus.Disponivel;
    public IFormFile? Image { get; set; }
    public List<string>? PhotoUrls { get; set; } = new();

    public string? ImageUrl { get; set; }

    [Required]
    [StringLength(100)]
    public string City { get; set; }

    [Required]
    [StringLength(100)]
    public string State { get; set; }

    [Required]
    public string CreatedByUserId { get; set; }
}
