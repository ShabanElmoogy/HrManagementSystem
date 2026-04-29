namespace HrManagementSystem.Contracts.Countries;

public record CountryRequest(
    int Id,
    string NameAr,
    string NameEn,
    string? Alpha2Code,
    string? Alpha3Code,
    string? PhoneCode,
    string? CurrencyCode
);