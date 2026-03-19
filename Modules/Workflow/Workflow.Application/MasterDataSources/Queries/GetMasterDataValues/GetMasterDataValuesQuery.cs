using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.MasterDataSources.Queries.GetMasterDataValues
{
    public record GetMasterDataValuesQuery(int SourceId, PagingRequest Request)
        : IRequest<PagingResponse<ViewMasterDataValueDto>>;
}