namespace HrManagementSystem.Contracts.BasicContracts.Categories
{
    public record SimpleCategoryResponse(
        int Id,
        string NameAr,
        string NameEn,
        bool IsDeleted);
}
