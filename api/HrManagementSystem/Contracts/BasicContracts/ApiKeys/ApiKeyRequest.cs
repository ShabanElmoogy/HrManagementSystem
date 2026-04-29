namespace HrManagementSystem.Contracts.BasicContracts.ApiKeys
{
    public record ApiKeyRequest(
        int Id,
        string Key,
        string ClientUri,
        string Description,
        bool IsActive,
        DateTime? ExpiresAt
    );
}