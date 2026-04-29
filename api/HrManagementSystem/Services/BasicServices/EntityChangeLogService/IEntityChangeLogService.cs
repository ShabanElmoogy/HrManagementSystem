using HrManagementSystem.Contracts.BasicContracts.EntityChangeLogs;

namespace HrManagementSystem.Services.BasicServices.EntityChangeLogService
{
    public interface IEntityChangeLogService
    {
        Task<EntityChangeLogsRequest> CreateChangeLogAsync<TEntity>(int entityId, TEntity existingEntity, TEntity updatedEntity) where TEntity : class;

        Task<List<EntityChangeLogsResponse>> GetChangeLogKeyValuesAsync();
    }
}
