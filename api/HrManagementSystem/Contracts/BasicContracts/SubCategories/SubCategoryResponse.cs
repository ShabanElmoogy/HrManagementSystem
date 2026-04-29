using HrManagementSystem.Contracts.BasicContracts.Categories;

namespace HrManagementSystem.Contracts.BasicContracts.SubCategories
{
    public record SubCategoryResponse(
        int Id,
        string NameAr,
        string NameEn,
        DateTime CreatedOn,
        DateTime? UpdatedOn,
        bool IsDeleted,
        List<SimpleCategoryResponse>? Categories = null
        );
}
