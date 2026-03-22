using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class UpdateActionCommandHandler : IRequestHandler<UpdateActionCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateActionCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(UpdateActionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Data;
            var userId = _currentUserService.UserId;

            var action = await _repository.GetActionByIdAsync(request.Id);
            if (action == null)
            {
                throw new NotFoundException("Không tìm thấy hành động bước quy trình.");
            }

            action.Update(
                label: dto.Label,
                targetStepId: dto.TargetStepId,
                notifyTemplate: dto.NotifyTemplate,
                sortOrder: dto.SortOrder,
                modifiedBy: userId
            );

            await _repository.SaveActionAsync(action);
            return true;
        }
    }
}
