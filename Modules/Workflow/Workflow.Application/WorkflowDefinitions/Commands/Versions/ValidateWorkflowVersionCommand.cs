using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Versions
{
    public class ValidateWorkflowVersionCommand : IRequest<WorkflowValidationResultDto>
    {
        public int Id { get; set; }

        public ValidateWorkflowVersionCommand(int id)
        {
            Id = id;
        }
    }
}