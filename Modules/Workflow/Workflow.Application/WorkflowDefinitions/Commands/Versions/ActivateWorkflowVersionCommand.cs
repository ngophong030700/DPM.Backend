using MediatR;

namespace Workflow.Application.WorkflowDefinitions.Commands.Versions
{
    public class ActivateWorkflowVersionCommand : IRequest<bool>
    {
        public int WorkflowId { get; set; }
        public int VersionId { get; set; }

        public ActivateWorkflowVersionCommand(int workflowId, int versionId)
        {
            WorkflowId = workflowId;
            VersionId = versionId;
        }
    }
}