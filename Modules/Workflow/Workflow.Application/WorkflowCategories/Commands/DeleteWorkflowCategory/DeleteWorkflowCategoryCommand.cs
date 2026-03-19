using MediatR;

namespace Workflow.Application.WorkflowCategories.Commands.DeleteWorkflowCategory
{
    public record DeleteWorkflowCategoryCommand(int Id) : IRequest<bool>;
}