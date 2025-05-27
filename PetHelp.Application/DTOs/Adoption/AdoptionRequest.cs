using System.ComponentModel.DataAnnotations;

namespace PetHelp.Application.DTOs.Adoption;

public class AdoptionRequest
{
    [Required(ErrorMessage = "O ID do animal é obrigatório.")]
    public Guid AnimalId { get; set; }

    [Required(ErrorMessage = "O nome completo é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome completo deve ter no máximo 100 caracteres.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O email fornecido é inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [Phone(ErrorMessage = "O telefone fornecido é inválido.")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "O endereço é obrigatório.")]
    public string Address { get; set; } = string.Empty;

    public bool HasOtherPets { get; set; }

    [Required(ErrorMessage = "O tipo de moradia é obrigatório.")]
    public string HousingType { get; set; } = string.Empty;

    [Range(1, 20, ErrorMessage = "O número de moradores deve ser entre 1 e 20.")]
    public int NumberOfResidents { get; set; }

    [Required(ErrorMessage = "O horário de trabalho é obrigatório.")]
    public string WorkSchedule { get; set; } = string.Empty;

    [Required(ErrorMessage = "O motivo da adoção é obrigatório.")]
    [StringLength(500, ErrorMessage = "O motivo da adoção deve ter no máximo 500 caracteres.")]
    public string ReasonForAdoption { get; set; } = string.Empty;

    [Range(typeof(bool), "true", "true", ErrorMessage = "Você deve concordar com os termos.")]
    public bool AgreedToTerms { get; set; }
}
