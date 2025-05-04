using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHelp.Application.Commands.Animals;

public class DeleteAnimalCommand
{
    public Guid Id { get; set; }
    public DeleteAnimalCommand(Guid id)
    {
        Id = id;
    }
}
