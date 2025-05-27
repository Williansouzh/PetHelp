using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelp.Application.DTOs.Animal;
using PetHelp.Application.Interfaces;
using PetHelp.Application.Pagination;

namespace PetHelp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalService _animalService;
    private readonly ILogger<AnimalsController> _logger;
    private readonly IMapper _mapper;
    public AnimalsController(IAnimalService animalService, ILogger<AnimalsController> logger, IMapper mapper)
    {
        _animalService = animalService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpPost]
    [Authorize(Roles = "ONG")] 
    public async Task<IActionResult> CreateAnimal([FromForm] AnimalRequest createAnimalDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var userIdClaim = User.FindFirst("id") ??
                             User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                _logger.LogWarning("User not properly authenticated. Claims: {@Claims}",
                    User.Claims.Select(c => new { c.Type, c.Value }));
                return Unauthorized("User not properly authenticated.");
            }

            createAnimalDto.CreatedByUserId = userId.ToString(); 

            var createdAnimal = await _animalService.CreateAnimalAsync(createAnimalDto);
            return CreatedAtAction(nameof(GetAnimalById), new { id = createdAnimal.Id }, createdAnimal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating animal");
            return StatusCode(500, "An error occurred while creating the animal");
        }
    }
    [HttpGet]
    public async Task<ActionResult<PaginationResponse<AnimalDTO>>> GetAnimals(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        try
        {
            var pagination = new PaginationRequest { PageNumber = pageNumber, PageSize = pageSize };
            var animals = await _animalService.GetAllAnimalsAsync(pagination.PageNumber, pagination.PageSize);
            return Ok(animals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching animals");
            return StatusCode(500, "An error occurred while retrieving animals");
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
    [HttpPut("{id}")]
    [Authorize(Roles = "ONG")]
    public async Task<IActionResult> UpdateAnimal(Guid id, [FromBody] AnimalRequest updateAnimalDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updatedAnimal = await _animalService.UpdateAnimalAsync(id, updateAnimalDto);
            if (updatedAnimal == null)
                return NotFound();

            return Ok(updatedAnimal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating animal");
            return StatusCode(500, "An error occurred while updating the animal");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "ONG")]
    public async Task<IActionResult> DeleteAnimal(Guid id)
    {
        try
        {
            var deleted = await _animalService.DeleteAnimalAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting animal");
            return StatusCode(500, "An error occurred while deleting the animal");
        }
    }
}
