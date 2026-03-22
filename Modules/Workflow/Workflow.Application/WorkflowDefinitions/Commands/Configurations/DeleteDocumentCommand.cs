using MediatR;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class DeleteDocumentCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteDocumentCommand(int id)
        {
            Id = id;
        }
    }
}