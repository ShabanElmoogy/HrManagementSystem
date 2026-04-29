namespace HrManagementSystem.Contracts.Districts;

public record DistrictRequest(
    int Id,
    string NameAr,
    string NameEn,
    string Code,
    int StateId
);
