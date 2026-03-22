using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;
using Newtonsoft.Json;

using Workflow.Application.WorkflowDefinitions.Mappings;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class UpdateFieldCommandHandler : IRequestHandler<UpdateFieldCommand, FieldConfigDto?>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateFieldCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<FieldConfigDto?> Handle(UpdateFieldCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var field = await _repository.GetFieldByIdAsync(request.Id);
            if (field == null)
            {
                throw new NotFoundException("Không tìm thấy trường dữ liệu.");
            }

            var dto = request.Data;

            // Handle Object to JSON string
            var settings = dto.GridSettings ?? dto.Settings;
            var settingsJson = settings != null ? JsonConvert.SerializeObject(settings) : dto.SettingsJson;
            var dsConfig = dto.Config ?? dto.DataSourceConfig;
            var dataSourceConfigJson = dsConfig != null ? JsonConvert.SerializeObject(dsConfig) : dto.DataSourceConfigJson;

            field.Update(
                label: dto.Label,
                dataSourceType: dto.DataSourceType,
                dataSourceConfigJson: dataSourceConfigJson,
                fieldFormula: dto.FieldFormula,
                settingsJson: settingsJson,
                sortOrder: dto.SortOrder,
                isRequired: dto.IsRequired,
                modifiedBy: userId
            );

            // Sync Grid Columns
            if (dto.GridColumns != null)
            {
                // 1. Delete removed columns (Only for real IDs < int.MaxValue)
                var incomingIds = dto.GridColumns
                    .Where(c => c.Id.HasValue && c.Id < int.MaxValue)
                    .Select(c => (int)c.Id!.Value)
                    .ToList();
                
                var columnsToDelete = field.GridColumns.Where(c => !incomingIds.Contains(c.Id) && !c.IsDeleted).ToList();
                foreach (var col in columnsToDelete) col.SoftDelete(userId);

                // 2. Update or Add
                foreach (var colDto in dto.GridColumns)
                {
                    var colDataType = WorkflowDefinitionMapping.MapToEnum(colDto.DataType);
                    var colDSConfig = colDto.Config ?? colDto.DataSourceConfig;
                    var colSettingsJson = colDto.Settings != null ? JsonConvert.SerializeObject(colDto.Settings) : colDto.SettingsJson;
                    var colDSConfigJson = colDSConfig != null ? JsonConvert.SerializeObject(colDSConfig) : colDto.DataSourceConfigJson;

                    if (colDto.Id.HasValue && colDto.Id < int.MaxValue)
                    {
                        var existingCol = field.GridColumns.FirstOrDefault(c => c.Id == (int)colDto.Id.Value);
                        existingCol?.Update(
                            label: colDto.Label,
                            dataSourceType: colDto.DataSourceType,
                            dataSourceConfigJson: colDSConfigJson,
                            settingsJson: colSettingsJson,
                            sortOrder: colDto.SortOrder,
                            isRequired: colDto.IsRequired,
                            modifiedBy: userId
                        );
                    }
                    else
                    {
                        field.AddGridColumn(
                            name: colDto.Name,
                            label: colDto.Label,
                            dataType: colDataType,
                            dataSourceType: colDto.DataSourceType,
                            dataSourceConfigJson: colDSConfigJson,
                            settingsJson: colSettingsJson,
                            sortOrder: colDto.SortOrder,
                            isRequired: colDto.IsRequired,
                            createdBy: userId
                        );
                    }
                }
            }

            await _repository.SaveFieldAsync(field);
            return field.ToFieldConfigDto();
        }
    }
}