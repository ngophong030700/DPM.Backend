using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowCategories.Queries.GetWorkflowCategoryList
{
    public class GetWorkflowCategoryListQueryHandler
        : IRequestHandler<GetWorkflowCategoryListQuery, PagingResponse<ViewListWorkflowCategoryDto>>
    {
        private readonly IWorkflowCategoryQueryService _queryService;

        public GetWorkflowCategoryListQueryHandler(IWorkflowCategoryQueryService queryService)
        {
            _queryService = queryService;
        }

        public async Task<PagingResponse<ViewListWorkflowCategoryDto>> Handle(
            GetWorkflowCategoryListQuery request,
            CancellationToken cancellationToken)
        {
            return await _queryService.GetListAsync(request.Request);
        }
    }
}