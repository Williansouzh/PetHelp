using MediatR;
using PetHelp.Application.Animals.Commands;
using PetHelp.Domain.Entities;
using PetHelp.Domain.Interfaces.Repositories;
using PetHelp.Domain.Interfaces.Services;

namespace PetHelp.Application.Animals.Handlers;

public class AnimalCreateCommandHandler : IRequestHandler<CreateAnimalCommand, Animal>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnimalRepository _animalRepository;
    public AnimalCreateCommandHandler(IUnitOfWork unitOfWork, IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Animal> Handle(CreateAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal = new Animal(
            request.Name, 
            request.Species, 
            request.Breed, 
            request.BirthDate, 
            request.Gender, 
            request.Size,
            request.Description, 
            request.IsVaccinated, 
            request.IsNeutered, 
            request.AdoptionRequirements,
            request.Status, 
            request.PhotoUrls, 
            request.ImageUrl, 
            request.City, 
            request.State, 
            request.CreatedByUserId, 
            request.AnimalType);
        var result = await _animalRepository.AddAsync(animal, cancellationToken);
        await _unitOfWork.CommitAsync();
        return result;
    }
}
