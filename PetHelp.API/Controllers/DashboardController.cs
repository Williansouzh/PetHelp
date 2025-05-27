using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelp.Application.DTOs.Dashboard;
using PetHelp.Domain.Interfaces.Repositories;
using static PetHelp.Domain.Enum.AnimalEnums;
using static PetHelp.Domain.Enum.ReportEnums;

namespace PetHelp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly ILogger<DashboardController> _logger;
    private readonly IAnimalRepository _animalRepository;
    private readonly IReportRepository _reportRepository;

    public DashboardController(
        ILogger<DashboardController> logger,
        IAnimalRepository animalRepository,
        IReportRepository reportRepository)
    {
        _logger = logger;
        _animalRepository = animalRepository;
        _reportRepository = reportRepository;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetDashboardStats()
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        try
        {
            var total = await _animalRepository.CountByUser(userId);
            var available = await _animalRepository.CountByUserAndStatus(userId, AnimalStatus.Disponivel);
            var adopted = await _animalRepository.CountByUserAndStatus(userId, AnimalStatus.Adotado);

            var stats = new DashboardStatsDto
            {
                TotalPets = total,
                AvailablePets = available,
                AdoptedPets = adopted
            };

            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching dashboard statistics for user {UserId}", userId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("recent-pets")]
    public async Task<IActionResult> GetRecentPets([FromQuery] int limit = 4)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        try
        {
            var pets = await _animalRepository.GetRecentByUser(userId, limit);

            var result = pets.Select(pet => new PetDashboardDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Breed = pet.Breed,
                Age = CalculateAge(pet.BirthDate),
                Status = pet.Status.ToString().ToLower(),
                ImageUrl = pet.ImageUrl,
                CreatedAt = pet.CreatedAt
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching recent pets for user {UserId}", userId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("recent-reports")]
    public async Task<IActionResult> GetRecentReports([FromQuery] int limit = 3)
    {
        try
        {
            var reports = await _reportRepository.GetRecentReports(limit);

            var result = reports.Select(r => new ReportDashboardDto
            {
                Id = r.Id,
                UrgencyLevel = r.UrgencyLevel.ToString().ToLower(),
                AnimalType = r.AnimalType.ToString().ToLower(),
                CreatedAt = r.CreatedAt
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching recent reports");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("pets")]
    public async Task<IActionResult> GetAllPets([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        try
        {
            var (pets, totalCount) = await _animalRepository.GetAllByUser(userId, page, pageSize);

            var dto = pets.Select(pet => new PetDashboardDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Breed = pet.Breed,
                Age = CalculateAge(pet.BirthDate),
                Status = pet.Status.ToString().ToLower(),
                ImageUrl = pet.ImageUrl,
                CreatedAt = pet.CreatedAt
            }).ToList();

            return Ok(new PaginationResponse<PetDashboardDto>(dto, page, pageSize, totalCount));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching paginated pets for user {UserId}", userId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("pets/available")]
    public async Task<IActionResult> GetAvailablePets()
    {
        return await HandleGet(() => _animalRepository.GetByStatus(AnimalStatus.Disponivel), "available pets");
    }

    [HttpGet("pets/species/{species}")]
    public async Task<IActionResult> GetPetsBySpecies(string species)
    {
        return await HandleGet(() => _animalRepository.GetBySpecies(species), $"pets by species: {species}");
    }

    [HttpGet("pets/size/{size}")]
    public async Task<IActionResult> GetPetsBySize(Size size)
    {
        return await HandleGet(() => _animalRepository.GetBySize(size), $"pets by size: {size}");
    }

    [HttpGet("reports/urgency/{urgencyLevel}")]
    public async Task<IActionResult> GetReportsByUrgencyLevel(UrgencyLevel urgencyLevel)
    {
        return await HandleGet(() => _reportRepository.GetByUrgencyLevel(urgencyLevel), $"reports by urgency: {urgencyLevel}");
    }

    [HttpGet("reports/animal-type/{animalType}")]
    public async Task<IActionResult> GetReportsByAnimalType(AnimalType animalType)
    {
        return await HandleGet(() => _reportRepository.GetByAnimalType(animalType), $"reports by animal type: {animalType}");
    }

    // Helpers

    private string? GetUserId()
    {
        return User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value;
    }

    private static string CalculateAge(DateTime? birthDate)
    {
        if (!birthDate.HasValue) return "Unknown";

        var today = DateTime.Today;
        var age = today.Year - birthDate.Value.Year;

        if (birthDate.Value.Date > today.AddYears(-age)) age--;

        return age == 1 ? "1 year" : $"{age} years";
    }

    private async Task<IActionResult> HandleGet<T>(Func<Task<IEnumerable<T>>> operation, string context)
    {
        try
        {
            var result = await operation();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching {Context}", context);
            return StatusCode(500, "Internal server error");
        }
    }
}
