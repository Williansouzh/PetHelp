using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelp.API.DTOs.ReportDTOs;
using PetHelp.Application.DTOs.Animal;
using PetHelp.Application.DTOs.Report;
using PetHelp.Application.Interfaces;
using PetHelp.Application.Pagination;

namespace PetHelp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly ILogger<ReportsController> _logger;
    private readonly IReportService _reportService;
    private readonly IMapper _mapper;
    public ReportsController(ILogger<ReportsController> logger, IReportService reportService, IMapper mapper)
    {
        _logger = logger;
        _reportService = reportService;
        _mapper = mapper;
    }
    [HttpPost]
    public async Task<IActionResult> CreateReport([FromForm] ReportRequestDTO requestDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createReportDTO = _mapper.Map<CreateReportDTO>(requestDTO);
            var createdReport = await _reportService.CreateReportAsync(createReportDTO, requestDTO.Imagem);
            _logger.LogInformation("Report created successfully with ID: {Id}", createdReport.Id);
            return CreatedAtAction(nameof(GetReportById), new { id = createdReport.Id }, createdReport);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating report");
            return StatusCode(500, "An error occurred while creating the report");
        }
    }
    [HttpGet("{id}")]
    [Authorize("ONG")]
    public async Task<IActionResult> GetReportById(Guid id)
    {
        var report = await _reportService.GetReportByIdAsync(id);
        if (report == null)
            return NotFound();

        return Ok(report);
    }
    [HttpGet]
    public async Task<ActionResult<PaginationResponse<AnimalDTO>>> GetAllReportsAsync(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        try
        {
            var pagination = new PaginationRequest { PageNumber = pageNumber, PageSize = pageSize };
            var animals = await _reportService.GetAllReportsAsync(pagination.PageNumber, pagination.PageSize);
            return Ok(animals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching animals");
            return StatusCode(500, "An error occurred while retrieving animals");
        }
    }
    [HttpPut("{id}")]
    [Authorize("ONG")]
    public async Task<IActionResult> UpdateReport(Guid id, [FromBody] UpdateReportDTO updateReportDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var updatedReport = await _reportService.UpdateReportAsync(id, updateReportDto);
            if (updatedReport == null)
                return NotFound();
            return Ok(updatedReport);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating report with ID: {Id}", id);
            return StatusCode(500, "An error occurred while updating the report");
        }
    }
    [HttpDelete("{id}")]
    [Authorize("ONG")]
    public async Task<IActionResult> DeleteReport(Guid id)
    {
        try
        {
            var deleted = await _reportService.DeleteReportAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting report with ID: {Id}", id);
            return StatusCode(500, "An error occurred while deleting the report");
        }
    }
}
