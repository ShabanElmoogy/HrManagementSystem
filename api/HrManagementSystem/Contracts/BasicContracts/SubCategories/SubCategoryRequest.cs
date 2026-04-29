namespace HrManagementSystem.Contracts.BasicContracts.SubCategories
{
    public record SubCategoryRequest(
        int Id,
        string NameAr,
        string NameEn,
        List<int>? CategoryIds);
}
