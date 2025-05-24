using PetHelp.Application.DTOs.Animal;
using AutoMapper;
using PetHelp.Application.Animals.Commands;
namespace PetHelp.Application.Mappings;

public class DTOToCommandMappingProfile : Profile
{
    public DTOToCommandMappingProfile()
    {
        CreateMap<CreateAnimalDTO, CreateAnimalCommand>();
        CreateMap<UpdateAnimalDTO, UpdateAnimalCommand>();
        CreateMap<DeleteAnimalDTO, DeleteAnimalCommand>();
    }
}
