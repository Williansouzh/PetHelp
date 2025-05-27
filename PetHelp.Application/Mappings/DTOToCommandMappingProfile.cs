using PetHelp.Application.DTOs.Animal;
using AutoMapper;
using PetHelp.Application.Animals.Commands;
using PetHelp.Application.Reports.Commands;
using PetHelp.Application.DTOs.Report;
using PetHelp.Application.DTOs.Adoption;
using PetHelp.Application.Adoptions.Commads;
namespace PetHelp.Application.Mappings;

public class DTOToCommandMappingProfile : Profile
{
    public DTOToCommandMappingProfile()
    {
        CreateMap<CreateAnimalDTO, CreateAnimalCommand>();
        CreateMap<UpdateAnimalDTO, UpdateAnimalCommand>();
        CreateMap<DeleteAnimalDTO, DeleteAnimalCommand>();

        CreateMap<CreateReportDTO, CreateReportCommand>();
        CreateMap<UpdateReportDTO, UpdateReportCommand>();
        CreateMap<DeleteReportDTO, DeleteReportCommand>();

        CreateMap<AdoptionRequest, CreateAdoptionCommand>();
        CreateMap<AdoptionRequest, UpdateAdoptionCommand>();
        CreateMap<AdoptionRequest, DeleteAdoptionCommand>();
    }
}
