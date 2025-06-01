using PetHelp.Application.DTOs.Adoption;
using static PetHelp.Domain.Enum.AdoptionEnums;

namespace PetHelp.Application.Interfaces;

public interface IAdoptionService
{
    Task<PaginationResponse<AdoptionDTO>> GetAllAdoptionsAsync(int pageNumber, int pageSize);
    Task<AdoptionDTO> GetAdoptionByIdAsync(Guid id);
    Task<AdoptionDTO> CreateAdoptionAsync(AdoptionRequest createAdoptionDto, string userId);
    Task<AdoptionDTO> UpdateAdoptionAsync(Guid id, AdoptionRequest updateAdoptionDto);
    Task<AdoptionDTO?> UpdateStatusAsync(Guid id, AdoptionStatus status);

    Task<bool> DeleteAdoptionAsync(Guid id);
    Task<IEnumerable<AdoptionDTO>> GetAdoptionsByUserIdAsync(int userId);
    Task<IEnumerable<AdoptionDTO>> GetAdoptionsByCityAndStateAsync(string city, string state);
    Task<IEnumerable<AdoptionDTO>> GetAdoptionsBySpeciesAsync(string species);
    Task<IEnumerable<AdoptionDTO>> GetAdoptionsByBreedAsync(string breed);
    Task<IEnumerable<AdoptionDTO>> GetAdoptionsByAgeRangeAsync(int minAge, int maxAge);
    Task<IEnumerable<AdoptionDTO>> GetAdoptionsByCityAsync(string city);
    Task<IEnumerable<AdoptionDTO>> GetAdoptionsByStateAsync(string state);
}
