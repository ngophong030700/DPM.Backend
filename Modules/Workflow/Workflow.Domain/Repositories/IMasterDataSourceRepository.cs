using Workflow.Domain.MasterDataSources;

namespace Workflow.Domain.Repositories
{
    public interface IMasterDataSourceRepository
    {
        Task<MasterDataSource?> GetByIdAsync(int id);
        Task<MasterDataSource?> GetByCodeAsync(string code);
        Task<MasterDataSource> CreateAsync(MasterDataSource masterDataSource);
        Task<MasterDataSource> UpdateAsync(MasterDataSource masterDataSource);
        Task<bool> SoftDeleteAsync(int id, int modifiedBy);
        Task<bool> IsCodeExistsAsync(string code, int? excludeId = null);
        
        // Value methods
        Task<MasterDataValue?> GetValueByIdAsync(int valueId);
        Task AddValueAsync(MasterDataValue value);
        void UpdateValue(MasterDataValue value);
        Task<bool> SoftDeleteValueAsync(int valueId, int modifiedBy);
        
        // Column methods
        Task<MasterDataColumn?> GetColumnByIdAsync(int columnId);
        Task AddColumnAsync(MasterDataColumn column);
        void UpdateColumn(MasterDataColumn column);
        Task<bool> SoftDeleteColumnAsync(int columnId, int modifiedBy);

        // Batch Save
        Task SaveChangesAsync();
    }
}