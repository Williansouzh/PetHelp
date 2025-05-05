using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PetHelp.Application.DTOs.Animal;
using PetHelp.Domain.Entities;

namespace PetHelp.Application.Mappings;

public class DomainToDTOMappingProfile : Profile

{
    public DomainToDTOMappingProfile()
    {
        CreateMap<Animal, CreateAnimalDTO>();
        CreateMap<Animal, UpdateAnimalDTO>();
        CreateMap<Animal, DeleteAnimalDTO>();
        CreateMap<Animal, AnimalDTO>()
           .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => Guid.Parse(src.CreatedByUserId)));
    }
}
