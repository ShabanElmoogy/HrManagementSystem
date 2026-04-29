using HrManagementSystem.Contracts.BasicContracts.ApiKeys;

namespace HrManagementSystem.Services.BasicServices.ApiKeysService
{
    public interface IApiKeyService
    {
        Task<Result<ApiKeyResponse>> AddAsync(ApiKeyRequest apiKeyRequest);
        Task<Result<ApiKeyResponse>> UpdateAync(ApiKeyRequest updatedRequest);
        Task<Result<ApiKeyResponse>> GetApiKeyAsync(int id);
        Task<IEnumerable<ApiKeyResponse>> GetAllApiKeysAsync();
        Task<Result> RevokeApiKeyAsync(int id);
    }
}