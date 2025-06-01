using AutoMapper;
using MediatR;
using PetHelp.Application.Adoptions.Commads;
using PetHelp.Application.DTOs.Adoption;
using PetHelp.Domain.Entities;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Domain.Interfaces.Services;

namespace PetHelp.Application.Adoptions.Handlers;

public class CreateAdoptionCommandHandler : IRequestHandler<CreateAdoptionCommand, AdoptionDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAdoptionRepository _adoptionRepository;
    private readonly IAnimalRepository _animalRepository;
    private readonly IMapper _mapper;
    public CreateAdoptionCommandHandler(IUnitOfWork unitOfWork, IAdoptionRepository adoptionRepository, IMapper mapper, IAnimalRepository animalRepository)
    {
        _unitOfWork = unitOfWork;
        _adoptionRepository = adoptionRepository;
        _mapper = mapper;
        _animalRepository = animalRepository;
    }
    public async Task<AdoptionDTO> Handle(CreateAdoptionCommand request, CancellationToken cancellationToken)
    {
        var animal = await _animalRepository.GetByIdAsync(request.AnimalId);
        if (animal == null)
            throw new Exception("Animal not found");

        var adoption = new Adoption(
            request.AnimalId,
            request.UserId,
            request.FullName,
            animal.Name,
            request.Email,
            request.Phone,
            request.Address,
            request.HasOtherPets,
            request.HousingType,
            request.NumberOfResidents,
            request.WorkSchedule,
            request.ReasonForAdoption,
            request.AgreedToTerms
        );
        var result = await _adoptionRepository.AddAsync(adoption, cancellationToken);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<AdoptionDTO>(adoption);
    }
}
