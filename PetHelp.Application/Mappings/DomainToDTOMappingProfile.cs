using AutoMapper;
using PetHelp.Application.DTOs.Animal;
using PetHelp.Domain.Entities;

namespace PetHelp.Application.Mappings;

public class DomainToDTOMappingProfile : Profile
{
    public DomainToDTOMappingProfile()
    {
        CreateMap<Animal, AnimalDTO>().ReverseMap();
    }
}
