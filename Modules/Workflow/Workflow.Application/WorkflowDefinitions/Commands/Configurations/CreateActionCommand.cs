using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class CreateActionCommand : IRequest<bool>
    {
        public StepActionConfigDto Data { get; set; }

        public CreateActionCommand(StepActionConfigDto data)
        {
            Data = data;
        }
    }
}
