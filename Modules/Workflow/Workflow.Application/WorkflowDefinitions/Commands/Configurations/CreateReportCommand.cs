using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class CreateReportCommand : IRequest<bool>
    {
        public SetupWorkflowReportDto Data { get; set; }

        public CreateReportCommand(SetupWorkflowReportDto data)
        {
            Data = data;
        }
    }
}