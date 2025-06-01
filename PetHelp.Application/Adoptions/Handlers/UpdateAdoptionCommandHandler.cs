using AutoMapper;
using MediatR;
using PetHelp.Application.Adoptions.Commads;
using PetHelp.Application.DTOs.Adoption;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Domain.Interfaces.Services;
using static PetHelp.Domain.Enum.AdoptionEnums;

namespace PetHelp.Application.Adoptions.Handlers;

public class UpdateAdoptionCommandHandler : IRequestHandler<UpdateAdoptionCommand, AdoptionDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAdoptionRepository _adoptionRepository;
    private readonly IMapper _mapper;

    public UpdateAdoptionCommandHandler(IUnitOfWork unitOfWork, IAdoptionRepository adoptionRepository, IMapper mapper)
    {
        _adoptionRepository = adoptionRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AdoptionDTO> Handle(UpdateAdoptionCommand request, CancellationToken cancellationToken)
    {
        var adoption = await _adoptionRepository.GetByIdAsync(request.Id);
        if (adoption == null)
            throw new KeyNotFoundException($"Adoption with ID {request.Id} not found.");
        adoption.Update(
            request.FullName ?? adoption.FullName,
            request.AnimalName ?? adoption.AnimalName,
            request.Email ?? adoption.Email,
            request.Phone ?? adoption.Phone,
            request.Address ?? adoption.Address,
            request.HasOtherPets != default ? request.HasOtherPets : adoption.HasOtherPets,
            request.HousingType ?? adoption.HousingType,
            request.NumberOfResidents != default ? request.NumberOfResidents : adoption.NumberOfResidents,
            request.WorkSchedule ?? adoption.WorkSchedule,
            request.ReasonForAdoption ?? adoption.ReasonForAdoption,
            request.AgreedToTerms != default ? request.AgreedToTerms : adoption.AgreedToTerms
        );
        if (request.Status.HasValue)
        {
            if (request.Status == AdoptionStatus.Approved)
                adoption.Approve();
            else if (request.Status == AdoptionStatus.Rejected)
                adoption.Reject();
        }
        await _adoptionRepository.UpdateAsync(adoption);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<AdoptionDTO>(adoption);
    }
}
