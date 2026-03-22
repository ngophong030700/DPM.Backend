using MediatR;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class UpdateReportBasicCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public UpdateReportBasicCommand(int id, string name, bool isActive)
        {
            Id = id;
            Name = name;
            IsActive = isActive;
        }
    }

    public class UpdateReportConfigCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string? FieldsConfigJson { get; set; }
        public string? ChartConfigJson { get; set; }

        public UpdateReportConfigCommand(int id, string? fieldsConfigJson, string? chartConfigJson)
        {
            Id = id;
            FieldsConfigJson = fieldsConfigJson;
            ChartConfigJson = chartConfigJson;
        }
    }
}