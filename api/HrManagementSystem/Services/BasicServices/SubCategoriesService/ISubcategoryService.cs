using HrManagementSystem.Contracts.BasicContracts.SubCategories;

namespace HrManagementSystem.Services.BasicServices.SubCategoriesService
{
    public interface ISubCategoryService
    {
        Task<IEnumerable<SubCategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<SubCategoryResponse>> GetAllAsyncRelatedToCategeory(int CategoryId, CancellationToken cancellationToken = default);
        Task<Result<SubCategoryResponse>> GetAsync(int id, CancellationToken cancellationToken);
        Task<Result<SubCategoryResponse>> AddAsync(SubCategoryRequest request, CancellationToken cancellationToken = default);
        Task<Result<SubCategoryResponse>> UpdateAsync(SubCategoryRequest request, CancellationToken cancellationToken);
        Task<Result> ToggleAsync(int id, CancellationToken cancellationToken);
    }
}
