using MediatR;
using PetHelp.Application.Adoptions.Commads;
using PetHelp.Application.DTOs.Adoption;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Domain.Interfaces.Services;

namespace PetHelp.Application.Adoptions.Handlers;

public class UpdateStatusCommandHandler : IRequestHandler<UpdateStatusCommand, AdoptionDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAdoptionRepository _adoptionRepository;
    public UpdateStatusCommandHandler(IUnitOfWork unitOfWork, IAdoptionRepository adoptionRepository)
    {
        _unitOfWork = unitOfWork;
        _adoptionRepository = adoptionRepository;
    }
    public async Task<AdoptionDTO> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
    {
        var adoption = await _adoptionRepository.GetByIdAsync(request.AdoptionId);
        if (adoption == null)
            throw new KeyNotFoundException($"Adoption with ID {request.AdoptionId} not found.");
        adoption.UpdateStatus(request.Status);
        adoption.UpdatedAt = DateTime.UtcNow;
        await _adoptionRepository.UpdateAsync(adoption);
        await _unitOfWork.CommitAsync();
        return new AdoptionDTO
        {
            Id = adoption.Id,
            AnimalId = adoption.AnimalId,
            UserId = adoption.UserId,
            FullName = adoption.FullName,
            AnimalName = adoption.AnimalName,
            Email = adoption.Email,
            Phone = adoption.Phone,
            Address = adoption.Address,
            HasOtherPets = adoption.HasOtherPets,
            HousingType = adoption.HousingType,
            NumberOfResidents = adoption.NumberOfResidents,
            WorkSchedule = adoption.WorkSchedule,
            ReasonForAdoption = adoption.ReasonForAdoption,
            AgreedToTerms = adoption.AgreedToTerms,
            Status = adoption.Status,
            CreatedAt = adoption.CreatedAt,
            UpdatedAt = adoption.UpdatedAt
        };
    }
}
