using MediatR;
using Shared.Application.Common.Interfaces;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class CreateActionCommandHandler : IRequestHandler<CreateActionCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public CreateActionCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(CreateActionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Data;
            var userId = _currentUserService.UserId;

            var action = WorkflowStepDefineAction.Create(
                stepId: dto.StepId,
                buttonKey: dto.ButtonKey,
                label: dto.Label,
                targetStepId: dto.TargetStepId,
                notifyTemplate: dto.NotifyTemplate,
                createdBy: userId,
                sortOrder: dto.SortOrder
            );

            await _repository.SaveActionAsync(action);
            return true;
        }
    }
}
