using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;

using Workflow.Application.WorkflowDefinitions.Mappings;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class SetupWorkflowFieldsCommandHandler : IRequestHandler<SetupWorkflowFieldsCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public SetupWorkflowFieldsCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(SetupWorkflowFieldsCommand request, CancellationToken cancellationToken)
        {
            var version = await _repository.GetVersionByIdAsync(request.VersionId);
            if (version == null)
            {
                throw new NotFoundException("Không tìm thấy phiên bản quy trình.");
            }

            var userId = _currentUserService.UserId;
            var newFields = new List<WorkflowField>();

            foreach (var dto in request.Data.Fields)
            {
                var dataType = WorkflowDefinitionMapping.MapToEnum(dto.DataType);
                var field = WorkflowField.Create(
                    versionId: request.VersionId,
                    name: dto.Name,
                    label: dto.Label,
                    dataType: dataType,
                    dataSourceType: dto.DataSourceType,
                    dataSourceConfigJson: dto.DataSourceConfigJson,
                    fieldFormula: dto.FieldFormula,
                    settingsJson: dto.SettingsJson,
                    sortOrder: dto.SortOrder,
                    isRequired: dto.IsRequired,
                    createdBy: userId
                );

                if (dto.Id.HasValue && dto.Id.Value > 0 && dto.Id.Value < int.MaxValue)
                {
                    typeof(WorkflowField).GetField("_id", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.SetValue(field, (int)dto.Id.Value);
                }

                if (dataType == FieldDataType.Grid && dto.GridColumns.Any())
                {
                    foreach (var col in dto.GridColumns)
                    {
                        field.AddGridColumn(
                            name: col.Name,
                            label: col.Label,
                            dataType: WorkflowDefinitionMapping.MapToEnum(col.DataType),
                            dataSourceType: col.DataSourceType,
                            dataSourceConfigJson: col.DataSourceConfigJson,
                            settingsJson: col.SettingsJson,
                            sortOrder: col.SortOrder,
                            isRequired: col.IsRequired,
                            createdBy: userId
                        );
                    }
                }

                newFields.Add(field);
            }

            await _repository.SaveFieldsAsync(request.VersionId, newFields);

            return true;
        }
    }
}