using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetHelp.API.DTOs.ReportDTOs;
using PetHelp.Application.DTOs.Animal;
using PetHelp.Application.DTOs.Report;
using PetHelp.Application.Interfaces;

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
    public async Task<IActionResult> CreateReport([FromBody] ReportRequestDTO requestDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var createReportDTO = _mapper.Map<CreateReportDTO>(requestDTO);
            var createdReport = await _reportService.CreateReportAsync(createReportDTO);
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
    public async Task<IActionResult> GetReportById(Guid id)
    {
        var report = await _reportService.GetReportByIdAsync(id);
        if (report == null)
            return NotFound();

        return Ok(report);
    }
}
