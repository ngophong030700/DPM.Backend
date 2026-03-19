using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;
using Workflow.Application.MasterDataSources.Queries;

namespace Workflow.Application.MasterDataSources.Queries.GetMasterDataSourceList
{
    public class GetMasterDataSourceListQueryHandler
        : IRequestHandler<GetMasterDataSourceListQuery, PagingResponse<ViewListMasterDataSourceDto>>
    {
        private readonly IMasterDataSourceQueryService _queryService;

        public GetMasterDataSourceListQueryHandler(IMasterDataSourceQueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task<PagingResponse<ViewListMasterDataSourceDto>> Handle(
            GetMasterDataSourceListQuery request,
            CancellationToken cancellationToken)
        {
            return await _queryService.GetListAsync(request.Request);
        }
    }
}