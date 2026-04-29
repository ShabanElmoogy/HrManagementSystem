namespace HrManagementSystem.Contracts.BasicContracts.SubCategories
{
    public record SimpleSubCategoryResponse(
        int Id,
        string NameAr,
        string NameEn,
        bool IsDeleted
        );
}
