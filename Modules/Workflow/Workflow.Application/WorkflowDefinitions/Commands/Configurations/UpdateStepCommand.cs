using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class UpdateStepCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public int VersionId { get; set; }
        public StepConfigDto Data { get; set; }

        public UpdateStepCommand(string id, int versionId, StepConfigDto data)
        {
            Id = id;
            VersionId = versionId;
            Data = data;
        }
    }
}
