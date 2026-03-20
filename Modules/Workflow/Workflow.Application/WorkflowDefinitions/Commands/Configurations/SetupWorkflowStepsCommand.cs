using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class SetupWorkflowStepsCommand : IRequest<bool>
    {
        public int VersionId { get; set; }
        public SetupWorkflowStepsDto Data { get; set; }

        public SetupWorkflowStepsCommand(int versionId, SetupWorkflowStepsDto data)
        {
            VersionId = versionId;
            Data = data;
        }
    }
}