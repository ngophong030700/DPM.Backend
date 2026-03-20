using MediatR;
using Shared.Application.BaseClass;

namespace Workflow.Application.WorkflowDefinitions.Commands.DeleteWorkflowDefinition
{
    public class DeleteWorkflowDefinitionCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteWorkflowDefinitionCommand(int id)
        {
            Id = id;
        }
    }
}