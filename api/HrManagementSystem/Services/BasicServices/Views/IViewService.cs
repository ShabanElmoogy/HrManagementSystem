using HrManagementSystem.Contracts.BasicContracts.Views;

namespace HrManagementSystem.Services.BasicServices.Views
{
    public interface IViewService
    {
        Task CreateOrAlterViewAsync(ViewRequest view);
        Task<List<ViewResponse>> GetAllViewsAsync();
        Task DropViewAsync(string viewName);
        Task<List<string>> GetAllTablesAsync();
        Task<List<string>> GetTableColumnsAsync(string tableName);
    }
}
