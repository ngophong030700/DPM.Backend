using MediatR;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class DeleteStepCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public int VersionId { get; set; }

        public DeleteStepCommand(string id, int versionId)
        {
            Id = id;
            VersionId = versionId;
        }
    }
}
