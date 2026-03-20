using MediatR;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.CreateWorkflowDefinition
{
    public class CreateWorkflowDefinitionCommand : IRequest<ViewDetailWorkflowDefinitionDto?>
    {
        public CreateWorkflowDefinitionDto Data { get; set; }

        public CreateWorkflowDefinitionCommand(CreateWorkflowDefinitionDto data)
        {
            Data = data;
        }
    }
}