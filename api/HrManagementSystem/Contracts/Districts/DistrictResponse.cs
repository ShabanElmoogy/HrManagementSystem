namespace HrManagementSystem.Contracts.Districts;

public record DistrictResponse(
    int Id,
    string NameAr,
    string NameEn,
    string Code,
    int StateId,
    DateTime CreatedOn,
    DateTime? UpdatedOn,
    SimpleStateResponse State,
    bool IsDeleted
);

