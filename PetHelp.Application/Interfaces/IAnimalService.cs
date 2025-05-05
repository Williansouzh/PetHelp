using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetHelp.Application.DTOs.Animal;

namespace PetHelp.Application.Interfaces;

public interface IAnimalService
{
    Task<IEnumerable<AnimalDTO>> GetAllAnimalsAsync();
    Task<AnimalDTO> GetAnimalByIdAsync(Guid id);
    Task<CreateAnimalDTO> CreateAnimalAsync(CreateAnimalDTO CreateAnimalDto);
    Task<UpdateAnimalDTO> UpdateAnimalAsync(UpdateAnimalDTO animalDto);
    Task<bool> DeleteAnimalAsync(Guid id);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByUserIdAsync(int userId);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByCityAndStateAsync(string city, string state);
    Task<IEnumerable<AnimalDTO>> GetAnimalsBySpeciesAsync(string species);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByBreedAsync(string breed);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByAgeRangeAsync(int minAge, int maxAge);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByCityAsync(string city);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByStateAsync(string state);

}
