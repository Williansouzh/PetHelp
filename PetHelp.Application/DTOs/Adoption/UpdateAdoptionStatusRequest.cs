using System.ComponentModel.DataAnnotations;
using static PetHelp.Domain.Enum.AdoptionEnums;

namespace PetHelp.Application.DTOs.Adoption;

public class UpdateAdoptionStatusRequest
{
    [Required]
    [EnumDataType(typeof(AdoptionStatus))]
    public AdoptionStatus Status { get; set; }
}
