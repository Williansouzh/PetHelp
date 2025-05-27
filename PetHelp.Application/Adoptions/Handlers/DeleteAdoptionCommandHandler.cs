using AutoMapper;
using MediatR;
using PetHelp.Application.Adoptions.Commads;
using PetHelp.Application.DTOs.Adoption;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Domain.Interfaces.Services;

namespace PetHelp.Application.Adoptions.Handlers;

public class DeleteAdoptionCommandHandler : IRequestHandler<DeleteAdoptionCommand, AdoptionDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnimalRepository _animalRepository;
    private readonly IMapper _mapper;
    public DeleteAdoptionCommandHandler( IUnitOfWork unitOfWork, IAnimalRepository animalRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _animalRepository = animalRepository;
        _mapper = mapper;
    }
    public async Task<AdoptionDTO> Handle(DeleteAdoptionCommand request, CancellationToken cancellationToken)
    {
        var adoption = await _animalRepository.GetByIdAsync(request.Id);
        if (adoption == null)
        {
            throw new KeyNotFoundException($"Adoption with ID {request.Id} not found.");
        }
        await _animalRepository.DeleteAsync(adoption.Id);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<AdoptionDTO>(adoption);
    }
}
