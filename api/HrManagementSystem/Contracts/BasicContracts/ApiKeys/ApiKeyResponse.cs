namespace HrManagementSystem.Contracts.BasicContracts.ApiKeys
{
    public record ApiKeyResponse(
        int Id,
        string Key,
        string ClientUri,
        string Description,
        bool IsActive,
        DateTime CreatedAt,
        DateTime? ExpiresAt
    );
}
