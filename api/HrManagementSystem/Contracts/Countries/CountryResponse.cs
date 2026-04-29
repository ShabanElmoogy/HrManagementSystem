using HrManagementSystem.Contracts.States;

namespace HrManagementSystem.Contracts.Countries;

public record CountryResponse(
    int Id,
    string NameAr,
    string NameEn,
    string Alpha2Code,
    string Alpha3Code,
    string PhoneCode,
    string CurrencyCode,
    List<SimpleStateResponse> States,
    DateTime CreatedOn,
    DateTime? UpdatedOn,
    bool IsDeleted
);
