using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Queries.GetWorkflowDefinitionById
{
    public class GetWorkflowDefinitionByIdQueryHandler : IRequestHandler<GetWorkflowDefinitionByIdQuery, ViewDetailWorkflowDefinitionDto?>
    {
        private readonly IWorkflowDefinitionQueryService _queryService;

        public GetWorkflowDefinitionByIdQueryHandler(IWorkflowDefinitionQueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task<ViewDetailWorkflowDefinitionDto?> Handle(GetWorkflowDefinitionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _queryService.GetByIdAsync(request.Id);
        }
    }
}