namespace HrManagementSystem.Contracts.State;

public record StateResponse(
    int Id,
    string NameAr,
    string NameEn,
    string Code,
    SimpleCountryResponse Country,
    DateTime CreatedOn,
    DateTime? UpdatedOn,
    bool IsDeleted
);