namespace HrManagementSystem.Authentication
{
    public class JwtOptions
    {
        [Required]
        public string Key { get; init; }

        [Required]
        public string Issuer { get; init; }

        [Required]
        public string Audience { get; init; }

        [Range(1, int.MaxValue, ErrorMessage = "Invalid ExpireInMintues")]
        public long ExpireInMintues { get; init; }
    }
}
