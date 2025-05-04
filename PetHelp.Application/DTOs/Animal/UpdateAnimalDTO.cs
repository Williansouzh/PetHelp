using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHelp.Application.DTOs.Animal;

public class UpdateAnimalDTO
{
    [Required(ErrorMessage = "O nome do animal é obrigatório.")]
    public string Name { get; set; }
    public string Species { get; set; }
    public string? Breed { get; set; }
    public int Age { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string City { get; set; }
    public string State { get; set; }
}
