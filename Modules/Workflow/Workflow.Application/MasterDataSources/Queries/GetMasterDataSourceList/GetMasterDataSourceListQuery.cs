using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.MasterDataSources.Queries.GetMasterDataSourceList
{
    public record GetMasterDataSourceListQuery(PagingRequest Request)
        : IRequest<PagingResponse<ViewListMasterDataSourceDto>>;
}