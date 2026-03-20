using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Versions
{
    public class CreateWorkflowVersionCommand : IRequest<ViewWorkflowVersionDto?>
    {
        public int WorkflowId { get; set; }
        public CreateWorkflowVersionDto Data { get; set; }

        public CreateWorkflowVersionCommand(int workflowId, CreateWorkflowVersionDto data)
        {
            WorkflowId = workflowId;
            Data = data;
        }
    }
}