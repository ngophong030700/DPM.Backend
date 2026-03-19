using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowCategories.Commands.CreateWorkflowCategory
{
    public record CreateWorkflowCategoryCommand(CreateWorkflowCategoryDto Dto)
        : IRequest<ViewDetailWorkflowCategoryDto?>;
}