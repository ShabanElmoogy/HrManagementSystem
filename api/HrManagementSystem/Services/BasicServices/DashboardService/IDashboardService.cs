using HrManagementSystem.Contracts.BasicContracts.Dashboard;

namespace HrManagementSystem.Services.BasicServices.DashboardService
{
    public interface IDashboardService
    {
        Task<Result<UsersCountResponse>> GetUsersCountAsync(CancellationToken cancellationToken = default);
    }
}
