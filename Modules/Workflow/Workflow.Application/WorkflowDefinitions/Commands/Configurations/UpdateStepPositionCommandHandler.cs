using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class UpdateStepPositionCommandHandler : IRequestHandler<UpdateStepPositionCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateStepPositionCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(UpdateStepPositionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Data;
            var userId = _currentUserService.UserId;

            var step = await _repository.GetStepByIdAsync(request.Id, request.VersionId);
            if (step == null)
            {
                throw new NotFoundException("Không tìm thấy bước quy trình.");
            }

            step.UpdatePosition(dto.PositionX, dto.PositionY, userId);

            await _repository.SaveStepAsync(step);
            return true;
        }
    }
}
