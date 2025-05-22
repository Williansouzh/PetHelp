using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using PetHelp.Domain.Entities;

namespace PetHelp.Application.Animals.Commands
{
    public class UpdateAnimalCommand : IRequest<Animal>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Species { get; set; }
        public string? Breed { get; set; }
        public int? Age { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}
