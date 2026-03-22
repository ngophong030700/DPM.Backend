using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class UpdateDocumentCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public StepDocumentConfigDto Data { get; set; }

        public UpdateDocumentCommand(int id, StepDocumentConfigDto data)
        {
            Id = id;
            Data = data;
        }
    }
}