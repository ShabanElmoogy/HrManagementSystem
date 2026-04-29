using HrManagementSystem.Entities.BasicEntities;

namespace HrManagementSystem.Entities.ApplicationEntities.GeographicDetails;

public class AddressType : AuditableEntity
{
    public int Id { get; set; }
    public string NameEn { get; set; } = null!;
    public string NameAr { get; set; } = null!;
    public ICollection<Address> Addresses { get; set; } = [];
}

// Examples
// Residential = 1,
// Commercial = 2,
// Office = 3,
// Warehouse = 4,
// Shipping = 5,
// Billing = 6,
// Emergency = 7,
// Other = 8
