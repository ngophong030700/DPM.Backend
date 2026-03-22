using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Versions
{
    public class CloneWorkflowVersionCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public CloneWorkflowVersionDto Data { get; set; }

        public CloneWorkflowVersionCommand(int id, CloneWorkflowVersionDto data)
        {
            Id = id;
            Data = data;
        }
    }

    public class ActivateWorkflowVersionByIdCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public ActivateWorkflowVersionByIdCommand(int id)
        {
            Id = id;
        }
    }

    public class DeleteWorkflowVersionCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteWorkflowVersionCommand(int id)
        {
            Id = id;
        }
    }
}