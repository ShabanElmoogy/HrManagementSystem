namespace HrManagementSystem.Contracts.Addresses;

public record AddressResponse(
    int Id,
    string BuildingNumber,
    string Floor,
    string ApartmentNumber,
    string PostalCode,
    string AdditionalInfo,
    double Latitude,
    double Longitude,
    bool IsDefault,
    int AddressTypeId,
    int DistrictId,
    DateTime CreatedOn,
    DateTime? UpdatedOn,
    bool IsDeleted
);