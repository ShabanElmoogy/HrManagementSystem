namespace HrManagementSystem.Entities.BasicEntities
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn || RevokedOn is not null;
        public bool IsActive => RevokedOn is null && !IsExpired;
    }
}
