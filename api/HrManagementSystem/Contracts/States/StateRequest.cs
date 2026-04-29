namespace HrManagementSystem.Contracts.State;

public record StateRequest(
    int Id,
    string NameAr,
    string NameEn,
    string Code,
    int CountryId
);
