using System.ComponentModel.DataAnnotations;

namespace PetHelp.Application.DTOs.Animal;

public class CreateAnimalDTO
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "O nome do animal é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "A espécie do animal é obrigatória.")]
    [StringLength(50, ErrorMessage = "A espécie deve ter no máximo 50 caracteres.")]
    public string Species { get; set; }

    [StringLength(100, ErrorMessage = "A raça deve ter no máximo 100 caracteres.")]
    public string? Breed { get; set; }

    [Range(0, 100, ErrorMessage = "A idade deve estar entre 0 e 100.")]
    public int Age { get; set; }

    [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres.")]
    public string? Description { get; set; }

    [Url(ErrorMessage = "A URL da imagem deve ser válida.")]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "A cidade é obrigatória.")]
    [StringLength(100, ErrorMessage = "A cidade deve ter no máximo 100 caracteres.")]
    public string City { get; set; }

    [Required(ErrorMessage = "O estado é obrigatório.")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "O estado deve ter exatamente 2 caracteres (ex: 'SP').")]
    public string State { get; set; }
    public string CreatedByUserId { get; set; }
}
