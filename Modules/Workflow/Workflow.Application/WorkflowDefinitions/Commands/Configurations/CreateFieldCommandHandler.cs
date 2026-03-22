using MediatR;
using Shared.Application.Common.Interfaces;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;
using Newtonsoft.Json;

using Workflow.Application.WorkflowDefinitions.Mappings;
using Shared.Application.DTOs.Workflows;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class CreateFieldCommandHandler : IRequestHandler<CreateFieldCommand, FieldConfigDto?>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public CreateFieldCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<FieldConfigDto?> Handle(CreateFieldCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Data;
            var userId = _currentUserService.UserId;

            // Mapping flexible DataType
            var dataType = WorkflowDefinitionMapping.MapToEnum(dto.DataType);
            
            // Handle Object to JSON string
            var settings = dto.GridSettings ?? dto.Settings;
            var settingsJson = settings != null ? JsonConvert.SerializeObject(settings) : dto.SettingsJson;
            var dsConfig = dto.Config ?? dto.DataSourceConfig;
            var dataSourceConfigJson = dsConfig != null ? JsonConvert.SerializeObject(dsConfig) : dto.DataSourceConfigJson;

            var versionId = dto.WorkflowVersionId ?? dto.VersionId;

            var field = WorkflowField.Create(
                versionId: versionId,
                name: dto.Name,
                label: dto.Label,
                dataType: dataType,
                dataSourceType: dto.DataSourceType,
                dataSourceConfigJson: dataSourceConfigJson,
                fieldFormula: dto.FieldFormula,
                settingsJson: settingsJson,
                sortOrder: dto.SortOrder,
                isRequired: dto.IsRequired,
                createdBy: userId
            );

            // Handle Grid Columns if any
            if (dto.GridColumns != null && dto.GridColumns.Any())
            {
                foreach (var colDto in dto.GridColumns)
                {
                    var colDataType = WorkflowDefinitionMapping.MapToEnum(colDto.DataType);
                    var colDSConfig = colDto.Config ?? colDto.DataSourceConfig;
                    var colSettingsJson = colDto.Settings != null ? JsonConvert.SerializeObject(colDto.Settings) : colDto.SettingsJson;
                    var colDSConfigJson = colDSConfig != null ? JsonConvert.SerializeObject(colDSConfig) : colDto.DataSourceConfigJson;

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

            await _repository.SaveFieldAsync(field);
            return field.ToFieldConfigDto();
        }
    }
}