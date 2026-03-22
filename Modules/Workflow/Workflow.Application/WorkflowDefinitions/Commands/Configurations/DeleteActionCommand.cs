using MediatR;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class DeleteActionCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteActionCommand(int id)
        {
            Id = id;
        }
    }
}
