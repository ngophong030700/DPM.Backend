using MediatR;
using Shared.Application.Common.Interfaces;
using Workflow.Domain.Repositories;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class ReorderFieldsCommandHandler : IRequestHandler<ReorderFieldsCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public ReorderFieldsCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(ReorderFieldsCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            foreach (var item in request.Data)
            {
                var field = await _repository.GetFieldByIdAsync(item.Id);
                if (field != null)
                {
                    field.Update(
                        label: null,
                        dataSourceType: null,
                        dataSourceConfigJson: null,
                        fieldFormula: null,
                        settingsJson: null,
                        sortOrder: item.SortOrder,
                        isRequired: null,
                        modifiedBy: userId
                    );
                    await _repository.SaveFieldAsync(field);
                }
            }
            return true;
        }
    }
}