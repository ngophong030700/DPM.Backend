using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowCategories.Queries.GetWorkflowCategoryList
{
    public record GetWorkflowCategoryListQuery(PagingRequest Request)
        : IRequest<PagingResponse<ViewListWorkflowCategoryDto>>;
}