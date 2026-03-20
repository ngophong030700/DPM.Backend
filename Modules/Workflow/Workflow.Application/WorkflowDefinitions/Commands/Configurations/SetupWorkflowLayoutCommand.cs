using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class SetupWorkflowLayoutCommand : IRequest<bool>
    {
        public int VersionId { get; set; }
        public SetupWorkflowLayoutDto Data { get; set; }

        public SetupWorkflowLayoutCommand(int versionId, SetupWorkflowLayoutDto data)
        {
            VersionId = versionId;
            Data = data;
        }
    }
}