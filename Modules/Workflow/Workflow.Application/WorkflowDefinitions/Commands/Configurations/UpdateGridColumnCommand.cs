using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class UpdateGridColumnCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public GridColumnConfigDto Data { get; set; }

        public UpdateGridColumnCommand(int id, GridColumnConfigDto data)
        {
            Id = id;
            Data = data;
        }
    }
}