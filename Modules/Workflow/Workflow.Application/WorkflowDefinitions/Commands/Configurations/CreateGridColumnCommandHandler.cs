using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;

using Workflow.Application.WorkflowDefinitions.Mappings;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class CreateGridColumnCommandHandler : IRequestHandler<CreateGridColumnCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public CreateGridColumnCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(CreateGridColumnCommand request, CancellationToken cancellationToken)
        {
            var field = await _repository.GetFieldByIdAsync(request.Data.FieldId);
            if (field == null)
            {
                throw new NotFoundException("Không tìm thấy trường dữ liệu.");
            }

            var userId = _currentUserService.UserId;
            var column = field.AddGridColumn(
                name: request.Data.Name,
                label: request.Data.Label,
                dataType: WorkflowDefinitionMapping.MapToEnum(request.Data.DataType),
                dataSourceType: request.Data.DataSourceType,
                dataSourceConfigJson: request.Data.DataSourceConfigJson,
                settingsJson: request.Data.SettingsJson,
                sortOrder: request.Data.SortOrder,
                isRequired: request.Data.IsRequired,
                createdBy: userId
            );

            await _repository.SaveGridColumnAsync(column);

            return true;
        }
    }
}