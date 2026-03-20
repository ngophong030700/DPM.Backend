using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class SetupWorkflowReportCommand : IRequest<bool>
    {
        public int VersionId { get; set; }
        public SetupWorkflowReportDto Data { get; set; }

        public SetupWorkflowReportCommand(int versionId, SetupWorkflowReportDto data)
        {
            VersionId = versionId;
            Data = data;
        }
    }
}