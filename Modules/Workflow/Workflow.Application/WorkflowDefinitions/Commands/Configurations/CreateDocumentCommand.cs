using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class CreateDocumentCommand : IRequest<bool>
    {
        public StepDocumentConfigDto Data { get; set; }

        public CreateDocumentCommand(StepDocumentConfigDto data)
        {
            Data = data;
        }
    }
}
