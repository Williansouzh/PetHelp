using System.ComponentModel.DataAnnotations;
using static PetHelp.Domain.Enum.AnimalEnums;

namespace PetHelp.Application.DTOs.Animal;

public class UpdateAnimalRequest
{
    [Required]
    [EnumDataType(typeof(AnimalStatus))]
    public AnimalStatus Status { get; set; }
}
