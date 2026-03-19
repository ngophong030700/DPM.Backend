using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.MasterDataSources.Queries
{
    public interface IMasterDataSourceQueryService
    {
        Task<ViewDetailMasterDataSourceDto?> GetByIdAsync(int id);
        Task<PagingResponse<ViewListMasterDataSourceDto>> GetListAsync(PagingRequest request);
        Task<PagingResponse<ViewMasterDataValueDto>> GetValuesAsync(int sourceId, PagingRequest request);
        Task<ViewMasterDataValueDto?> GetValueByIdAsync(int valueId);
    }
}