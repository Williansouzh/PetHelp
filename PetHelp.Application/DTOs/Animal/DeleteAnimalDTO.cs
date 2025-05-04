using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHelp.Application.DTOs.Animal;

public  class DeleteAnimalDTO
{
    public Guid Id { get; set; }
    public DeleteAnimalDTO(Guid id)
    {
        Id = id;
    }
}
