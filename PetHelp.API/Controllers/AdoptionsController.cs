using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelp.Application.DTOs.Adoption;
using PetHelp.Application.Interfaces;
using PetHelp.Domain.Enum;
using static PetHelp.Domain.Enum.AdoptionEnums;

namespace PetHelp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdoptionsController : ControllerBase
{
    private readonly IAdoptionService _adoptionService;
    private readonly ILogger<AdoptionsController> _logger;

    public AdoptionsController(IAdoptionService adoptionService, ILogger<AdoptionsController> logger)
    {
        _adoptionService = adoptionService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AdoptionRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var userId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("Usuário não autenticado.");
        var result = await _adoptionService.CreateAdoptionAsync(request, userId);
        return CreatedAtAction(nameof(GetById), new { id = result.UserId }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var adoption = await _adoptionService.GetAdoptionByIdAsync(id);
        if (adoption == null)
            return NotFound();

        return Ok(adoption);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] AdoptionRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _adoptionService.UpdateAdoptionAsync(id, request);
        if (updated == null)
            return NotFound();

        return Ok(updated);
    }
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateAdoptionStatusRequest request)
    {
        if (!Enum.IsDefined(typeof(AdoptionStatus), request.Status))
            return BadRequest("Invalid status value.");

        try
        {
            var updatedAdoption = await _adoptionService.UpdateStatusAsync(id, request.Status);

            return updatedAdoption is null
                ? NotFound()
                : Ok(updatedAdoption);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Failed to update adoption status for ID: {AdoptionId}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating adoption status for ID: {AdoptionId}", id);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _adoptionService.DeleteAdoptionAsync(id);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var pagedAdoptions = await _adoptionService.GetAllAdoptionsAsync(pageNumber, pageSize);
        return Ok(pagedAdoptions);
    }
}
