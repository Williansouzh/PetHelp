using static PetHelp.Domain.Enum.ReportEnums;
using System.ComponentModel.DataAnnotations;

namespace PetHelp.API.DTOs.ReportDTOs
{
    public class ReportRequestDTO
    {
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [MinLength(10, ErrorMessage = "A descrição deve ter no mínimo 10 caracteres.")]
        public string Description { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "A latitude é obrigatória.")]
        [Range(-90, 90, ErrorMessage = "Latitude deve estar entre -90 e 90.")]
        public float Latitude { get; set; }

        [Required(ErrorMessage = "A longitude é obrigatória.")]
        [Range(-180, 180, ErrorMessage = "Longitude deve estar entre -180 e 180.")]
        public float Longitude { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [MinLength(5, ErrorMessage = "O endereço deve ter no mínimo 5 caracteres.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "O tipo do animal é obrigatório.")]
        public AnimalType AnimalType { get; set; }

        [Required(ErrorMessage = "O nível de urgência é obrigatório.")]
        public UrgencyLevel UrgencyLevel { get; set; }

        public Guid? UserId { get; set; }

        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string? Name { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }
    }
}
