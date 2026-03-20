using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class SetupWorkflowFieldsCommand : IRequest<bool>
    {
        public int VersionId { get; set; }
        public SetupWorkflowFieldsDto Data { get; set; }

        public SetupWorkflowFieldsCommand(int versionId, SetupWorkflowFieldsDto data)
        {
            VersionId = versionId;
            Data = data;
        }
    }
}