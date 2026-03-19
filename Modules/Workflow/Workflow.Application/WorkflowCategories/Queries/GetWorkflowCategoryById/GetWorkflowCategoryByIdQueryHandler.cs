using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowCategories.Queries.GetWorkflowCategoryById
{
    public class GetWorkflowCategoryByIdQueryHandler
        : IRequestHandler<GetWorkflowCategoryByIdQuery, ViewDetailWorkflowCategoryDto?>
    {
        private readonly IWorkflowCategoryQueryService _queryService;

        public GetWorkflowCategoryByIdQueryHandler(IWorkflowCategoryQueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task<ViewDetailWorkflowCategoryDto?> Handle(
            GetWorkflowCategoryByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _queryService.GetByIdAsync(request.Id);
        }
    }
}