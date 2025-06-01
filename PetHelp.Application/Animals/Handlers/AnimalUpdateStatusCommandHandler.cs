
using AutoMapper;
using MediatR;
using PetHelp.Application.Animals.Commands;
using PetHelp.Application.DTOs.Animal;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Domain.Interfaces.Services;

namespace PetHelp.Application.Animals.Handlers;

public class AnimalUpdateStatusCommandHandler : IRequestHandler<UpdateAnimalStatusCommand, AnimalDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnimalRepository _animalRepository;
    private readonly IMapper _mapper;

    public AnimalUpdateStatusCommandHandler(IUnitOfWork unitOfWork, IAnimalRepository animalRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _animalRepository = animalRepository;
        _mapper = mapper;
    }

    public async Task<AnimalDTO> Handle(UpdateAnimalStatusCommand request, CancellationToken cancellationToken)
    {
        var animal = await _animalRepository.GetByIdAsync(request.AnimalId);
        if (animal == null)
            throw new KeyNotFoundException($"Animal with ID {request.AnimalId} not found.");

        animal.UpdateStatus(request.Status);
        animal.UpdatedAt = DateTime.UtcNow;

        await _animalRepository.UpdateAsync(animal);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<AnimalDTO>(animal);
    }
}
