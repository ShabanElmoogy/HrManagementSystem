using HrManagementSystem.Contracts.BasicContracts.ReportCategory;

namespace HrManagementSystem.Services.BasicServices.ReportCategoryService
{
    public interface IReportCategoryService
    {
        Task<IEnumerable<ReportCategoryResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result<ReportCategoryResponse>>? GetAsync(int id, CancellationToken cancellationToken);
        Task<Result<ReportCategoryResponse>> AddAsync(ReportCategoryRequest request, CancellationToken cancellationToken);
        Task<Result<ReportCategoryResponse>> UpdateAsync(ReportCategoryRequest request, CancellationToken cancellationToken = default);
        Task<Result> ToggleAsync(int id, CancellationToken cancellationToken);
    }
}
