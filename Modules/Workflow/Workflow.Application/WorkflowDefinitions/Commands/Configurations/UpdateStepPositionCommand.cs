using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class UpdateStepPositionCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public int VersionId { get; set; }
        public StepPositionDto Data { get; set; }

        public UpdateStepPositionCommand(string id, int versionId, StepPositionDto data)
        {
            Id = id;
            VersionId = versionId;
            Data = data;
        }
    }
}
