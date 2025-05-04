using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PetHelp.Application.DTOs.Animal;
using PetHelp.Domain.Entities;

namespace PetHelp.Application.Mappings;

public class DomainToDTOMapingProfile : Profile
{
    public DomainToDTOMapingProfile()
    {
        CreateMap<Animal, CreateAnimalDTO>();
        CreateMap<Animal, UpdateAnimalDTO>();
        CreateMap<Animal, DeleteAnimalDTO>();
    }
}
