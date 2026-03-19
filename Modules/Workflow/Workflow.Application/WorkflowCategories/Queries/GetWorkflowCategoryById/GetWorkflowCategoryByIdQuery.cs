using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowCategories.Queries.GetWorkflowCategoryById
{
    public record GetWorkflowCategoryByIdQuery(int Id) : IRequest<ViewDetailWorkflowCategoryDto?>;
}