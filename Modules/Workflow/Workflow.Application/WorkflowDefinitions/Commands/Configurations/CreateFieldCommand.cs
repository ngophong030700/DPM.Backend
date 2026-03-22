using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class CreateFieldCommand : IRequest<FieldConfigDto?>
    {
        public FieldConfigDto Data { get; set; }

        public CreateFieldCommand(FieldConfigDto data)
        {
            Data = data;
        }
    }
}