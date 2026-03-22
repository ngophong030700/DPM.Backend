using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class UpdateActionCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public StepActionConfigDto Data { get; set; }

        public UpdateActionCommand(int id, StepActionConfigDto data)
        {
            Id = id;
            Data = data;
        }
    }
}
