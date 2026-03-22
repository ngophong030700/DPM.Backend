using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class ReorderGridColumnsCommandHandler : IRequestHandler<ReorderGridColumnsCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public ReorderGridColumnsCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(ReorderGridColumnsCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            foreach (var item in request.Data)
            {
                var column = await _repository.GetGridColumnByIdAsync(item.Id);
                if (column != null)
                {
                    column.Update(
                        label: null,
                        dataSourceType: null,
                        dataSourceConfigJson: null,
                        settingsJson: null,
                        sortOrder: item.SortOrder,
                        isRequired: null,
                        modifiedBy: userId
                    );
                    await _repository.SaveGridColumnAsync(column);
                }
            }
            return true;
        }
    }
}