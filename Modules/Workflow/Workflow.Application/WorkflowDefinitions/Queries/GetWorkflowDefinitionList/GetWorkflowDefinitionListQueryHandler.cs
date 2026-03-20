using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Queries.GetWorkflowDefinitionList
{
    public class GetWorkflowDefinitionListQueryHandler : IRequestHandler<GetWorkflowDefinitionListQuery, PagingResponse<ViewListWorkflowDefinitionDto>>
    {
        private readonly IWorkflowDefinitionQueryService _queryService;

        public GetWorkflowDefinitionListQueryHandler(IWorkflowDefinitionQueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task<PagingResponse<ViewListWorkflowDefinitionDto>> Handle(GetWorkflowDefinitionListQuery request, CancellationToken cancellationToken)
        {
            return await _queryService.GetListAsync(request.Request);
        }
    }
}