using MediatR;
using Shared.Application.DTOs.Workflows;
using Workflow.Application.MasterDataSources.Queries;

namespace Workflow.Application.MasterDataSources.Queries.GetMasterDataSourceById
{
    public class GetMasterDataSourceByIdQueryHandler
        : IRequestHandler<GetMasterDataSourceByIdQuery, ViewDetailMasterDataSourceDto?>
    {
        private readonly IMasterDataSourceQueryService _queryService;

        public GetMasterDataSourceByIdQueryHandler(IMasterDataSourceQueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task<ViewDetailMasterDataSourceDto?> Handle(
            GetMasterDataSourceByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _queryService.GetByIdAsync(request.Id);
        }
    }
}