using MediatR;
using PetHelp.Application.Animals.Commands;
using PetHelp.Domain.Entities;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Domain.Interfaces.Services;

namespace PetHelp.Application.Animals.Handlers;

public class AnimalDeleteCommandHandler : IRequestHandler<DeleteAnimalCommand, Animal>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnimalRepository _animalRepository;
    public AnimalDeleteCommandHandler(IUnitOfWork unitOfWork, IAnimalRepository animalRepository)
    {
        _unitOfWork = unitOfWork;
        _animalRepository = animalRepository;
    }
    public async Task<Animal> Handle(DeleteAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal = await _animalRepository.GetByIdAsync(request.Id);
        if (animal == null)
        {
            throw new KeyNotFoundException($"Animal with ID {request.Id} not found.");
        }
        await _animalRepository.DeleteAsync(animal.Id);
        await _unitOfWork.CommitAsync();
        return animal;
    }
}
