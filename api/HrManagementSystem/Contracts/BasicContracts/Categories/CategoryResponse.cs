using HrManagementSystem.Contracts.BasicContracts.SubCategories;

namespace HrManagementSystem.Contracts.BasicContracts.Categories
{
    public record CategoryResponse(
        int Id,
        string NameAr,
        string NameEn,
        List<SimpleSubCategoryResponse> SubCategories,
        DateTime CreatedOn,
        DateTime? UpdatedOn,
        bool IsDeleted);
}
