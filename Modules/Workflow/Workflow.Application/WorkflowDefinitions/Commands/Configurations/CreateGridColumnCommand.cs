using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class CreateGridColumnCommand : IRequest<bool>
    {
        public GridColumnConfigDto Data { get; set; }

        public CreateGridColumnCommand(GridColumnConfigDto data)
        {
            Data = data;
        }
    }
}