using Microsoft.AspNetCore.Http;
using PetHelp.API.DTOs.ReportDTOs;
using PetHelp.Application.DTOs.Report;

namespace PetHelp.Application.Interfaces;

public interface IReportService
{
    Task<ReportDTO> CreateReportAsync(CreateReportDTO createReportDto, IFormFile imageFile);
    Task<IEnumerable<ReportResponseDTO>> GetAllReportsAsync(int pageNumber, int pageSize);
    Task<ReportDTO> GetReportByIdAsync(Guid id);
    Task<UpdateReportDTO> UpdateReportAsync(Guid id, UpdateReportDTO updateReportDto);
    Task<bool> DeleteReportAsync(Guid id);
}
