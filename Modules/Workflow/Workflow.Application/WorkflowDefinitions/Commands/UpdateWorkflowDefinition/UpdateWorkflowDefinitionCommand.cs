using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.UpdateWorkflowDefinition
{
    public class UpdateWorkflowDefinitionCommand : IRequest<ViewDetailWorkflowDefinitionDto?>
    {
        public int Id { get; set; }
        public UpdateWorkflowDefinitionDto Data { get; set; }

        public UpdateWorkflowDefinitionCommand(int id, UpdateWorkflowDefinitionDto data)
        {
            Id = id;
            Data = data;
        }
    }
}