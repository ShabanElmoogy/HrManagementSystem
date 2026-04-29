namespace HrManagementSystem.Entities.ApplicationEntities.GeographicDetails;

public class District : AuditableEntity
{
    public int Id { get; set; }
    public string NameEn { get; set; } =null!;
    public string NameAr { get; set; } =null!;
    public string Code { get; set; } = string.Empty;
    public int StateId { get; set; }
    public State State { get; set; } = null!;
    public ICollection<Address> Addresses { get; set; } = [];
}
