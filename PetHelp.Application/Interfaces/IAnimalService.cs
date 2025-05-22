using PetHelp.Application.DTOs.Animal;

namespace PetHelp.Application.Interfaces;

public interface IAnimalService
{
    Task<PaginationResponse<AnimalDTO>> GetAllAnimalsAsync(int pageNumber, int pageSize);
    Task<AnimalDTO> GetAnimalByIdAsync(Guid id);
    Task<CreateAnimalDTO> CreateAnimalAsync(CreateAnimalDTO CreateAnimalDto);
    Task<UpdateAnimalDTO> UpdateAnimalAsync(Guid id, UpdateAnimalDTO animalDto);
    Task<bool> DeleteAnimalAsync(Guid id);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByUserIdAsync(int userId);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByCityAndStateAsync(string city, string state);
    Task<IEnumerable<AnimalDTO>> GetAnimalsBySpeciesAsync(string species);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByBreedAsync(string breed);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByAgeRangeAsync(int minAge, int maxAge);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByCityAsync(string city);
    Task<IEnumerable<AnimalDTO>> GetAnimalsByStateAsync(string state);

}
