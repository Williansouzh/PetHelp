using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PetHelp.Application.Commands.Animals;
using PetHelp.Application.DTOs.Animal;
using PetHelp.Domain.Entities;

namespace PetHelp.Application.Mappings;

public class DomainToDTOMappingProfile : Profile

{
    public DomainToDTOMappingProfile()
    {
        CreateMap<Animal, CreateAnimalDTO>();
        CreateMap<UpdateAnimalDTO, UpdateAnimalCommand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Guid, DeleteAnimalCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));
        CreateMap<Animal, AnimalDTO>()
           .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => Guid.Parse(src.CreatedByUserId)));
    }
}
