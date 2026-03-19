using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;
using Workflow.Application.MasterDataSources.Queries;

namespace Workflow.Application.MasterDataSources.Queries.GetMasterDataValues
{
    public class GetMasterDataValuesQueryHandler
        : IRequestHandler<GetMasterDataValuesQuery, PagingResponse<ViewMasterDataValueDto>>
    {
        private readonly IMasterDataSourceQueryService _queryService;

        public GetMasterDataValuesQueryHandler(IMasterDataSourceQueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task<PagingResponse<ViewMasterDataValueDto>> Handle(
            GetMasterDataValuesQuery request,
            CancellationToken cancellationToken)
        {
            return await _queryService.GetValuesAsync(request.SourceId, request.Request);
        }
    }
}