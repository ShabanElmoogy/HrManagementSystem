using HrManagementSystem.Contracts.BasicContracts.Dashboard;

namespace HrManagementSystem.Services.BasicServices.DashboardService
{
    public class DashboardService(ApplicationDbContext context) : IDashboardService
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<Result<UsersCountResponse>> GetUsersCountAsync(CancellationToken cancellationToken = default)
        {
            var count = await _context.Users
                //.Where(x => !x.IsDisabled)
                .CountAsync(cancellationToken: cancellationToken);

            var response = new UsersCountResponse(count);

            return Result.Success(response);
        }

    }
}
