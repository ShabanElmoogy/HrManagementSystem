using HrManagementSystem.Contracts.BasicContracts.Categories;

namespace HrManagementSystem.Services.BasicServices.CategoriesService
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result<CategoryResponse>> GetAsync(int id, CancellationToken cancellationToken);
        Task<Result<CategoryResponse>> AddAsync(CategoryRequest request, CancellationToken cancellationToken = default);
        Task<Result<CategoryResponse>> UpdateAsync(CategoryRequest request, CancellationToken cancellationToken = default);
        Task<Result> ToggleAsync(int id, CancellationToken cancellationToken);
    }
}