using PetHelp.Application.DTOs.Animal;
using AutoMapper;
using PetHelp.Application.Animals.Commands;
using PetHelp.Application.Reports.Commands;
using PetHelp.Application.DTOs.Report;
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

    }
}
