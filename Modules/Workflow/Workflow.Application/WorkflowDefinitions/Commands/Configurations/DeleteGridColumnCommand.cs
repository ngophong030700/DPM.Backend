using MediatR;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class DeleteGridColumnCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteGridColumnCommand(int id)
        {
            Id = id;
        }
    }
}