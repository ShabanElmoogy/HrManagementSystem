namespace HrManagementSystem.Contracts.Countries;

public record SimpleCountryResponse(
    int Id,
    string NameAr,
    string NameEn,
    bool IsDeleted
    );