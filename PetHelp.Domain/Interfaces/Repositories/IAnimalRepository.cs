using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetHelp.Domain.Entities;

namespace PetHelp.Domain.Interfaces.Repositories
{
    public interface IAnimalRepository : IRepository<Animal>
    {
    }
}
