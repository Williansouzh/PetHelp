using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelp.Application.DTOs.Animal;
using PetHelp.Application.Interfaces;

namespace PetHelp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalService _animalService;
    private readonly ILogger<AnimalsController> _logger;
    public AnimalsController(IAnimalService animalService, ILogger<AnimalsController> logger)
    {
        _animalService = animalService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAnimal([FromBody] CreateAnimalDTO createAnimalDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            // Look for user ID in multiple possible claims
            var userIdClaim = User.FindFirst("id") ??
                             User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                _logger.LogWarning("User not properly authenticated. Claims: {@Claims}",
                    User.Claims.Select(c => new { c.Type, c.Value }));
                return Unauthorized("User not properly authenticated.");
            }

            createAnimalDto.CreatedByUserId = userId.ToString(); // Store as string if your DB uses GUID

            var createdAnimal = await _animalService.CreateAnimalAsync(createAnimalDto);
            return CreatedAtAction(nameof(GetAnimalById), new { id = createdAnimal.Id }, createdAnimal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating animal");
            return StatusCode(500, "An error occurred while creating the animal");
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAnimalById(Guid id)
    {
        var animal = await _animalService.GetAnimalByIdAsync(id);
        if (animal == null)
            return NotFound();
        return Ok(animal);
    }
}
