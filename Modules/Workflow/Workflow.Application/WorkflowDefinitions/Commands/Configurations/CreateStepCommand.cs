using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class CreateStepCommand : IRequest<bool>
    {
        public StepConfigDto Data { get; set; }

        public CreateStepCommand(StepConfigDto data)
        {
            Data = data;
        }
    }
}
