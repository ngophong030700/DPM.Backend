using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class UpdateGridColumnCommandHandler : IRequestHandler<UpdateGridColumnCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateGridColumnCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(UpdateGridColumnCommand request, CancellationToken cancellationToken)
        {
            var column = await _repository.GetGridColumnByIdAsync(request.Id);
            if (column == null)
            {
                throw new NotFoundException("Không tìm thấy cột lưới.");
            }

            column.Update(
                label: request.Data.Label,
                dataSourceType: request.Data.DataSourceType,
                dataSourceConfigJson: request.Data.DataSourceConfigJson,
                settingsJson: request.Data.SettingsJson,
                sortOrder: request.Data.SortOrder,
                isRequired: request.Data.IsRequired,
                modifiedBy: _currentUserService.UserId
            );

            await _repository.SaveGridColumnAsync(column);

            return true;
        }
    }
}