namespace HrManagementSystem.Contracts.States;

public record SimpleStateResponse(
    int Id,
    string NameAr,
    string NameEn,
    bool IsDeleted);
