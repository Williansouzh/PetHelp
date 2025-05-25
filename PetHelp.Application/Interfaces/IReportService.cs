using PetHelp.Application.DTOs.Report;

namespace PetHelp.Application.Interfaces;

public interface IReportService
{
    Task<ReportDTO> CreateReportAsync(CreateReportDTO createReportDto);
    Task<IEnumerable<ReportDTO>> GetAllReportsAsync();
    Task<ReportDTO> GetReportByIdAsync(Guid id);
    Task<UpdateReportDTO> UpdateReportAsync(Guid id, UpdateReportDTO updateReportDto);
    Task<bool> DeleteReportAsync(Guid id);
}
