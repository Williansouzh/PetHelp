using AutoMapper;
using PetHelp.API.DTOs.ReportDTOs;
using PetHelp.Application.DTOs.Adoption;
using PetHelp.Application.DTOs.Animal;
using PetHelp.Application.DTOs.Report;
using PetHelp.Domain.Entities;

namespace PetHelp.Application.Mappings;

public class DomainToDTOMappingProfile : Profile
{
    public DomainToDTOMappingProfile()
    {
        CreateMap<Animal, AnimalDTO>().ReverseMap();
        CreateMap<Report, ReportDTO>().ReverseMap();
        CreateMap<Adoption, AdoptionDTO>().ReverseMap();
        //error because ReportRequestDTO in o napi layer 
        CreateMap<ReportRequestDTO, CreateReportDTO>().ReverseMap();
    }
}
