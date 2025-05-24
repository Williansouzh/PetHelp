using MediatR;
using PetHelp.Application.Animals.Commands;
using PetHelp.Domain.Entities;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Domain.Interfaces.Services;

namespace PetHelp.Application.Animals.Handlers;

public class AnimalUpdateCommandHandler : IRequestHandler<UpdateAnimalCommand, Animal>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnimalRepository _animalRepository;
    public AnimalUpdateCommandHandler(IUnitOfWork unitOfWork, IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Animal> Handle(UpdateAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal = await _animalRepository.GetByIdAsync(request.Id);
        if (animal == null)
            throw new KeyNotFoundException($"Animal with ID {request.Id} not found.");

        if (request.Name != null) animal.Name = request.Name;
        if (request.Description != null) animal.Description = request.Description;
        if (request.Age.HasValue) animal.Age = request.Age.Value;
        if (request.Breed != null) animal.Breed = request.Breed;

        await _animalRepository.UpdateAsync(animal);
        await _unitOfWork.CommitAsync();
        return animal;
    }

}
