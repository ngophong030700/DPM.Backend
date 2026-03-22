using MediatR;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class DeleteFieldCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteFieldCommand(int id)
        {
            Id = id;
        }
    }
}