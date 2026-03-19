using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowCategories.Commands.UpdateWorkflowCategory
{
    public record UpdateWorkflowCategoryCommand(int Id, UpdateWorkflowCategoryDto Dto)
        : IRequest<ViewDetailWorkflowCategoryDto?>;
}