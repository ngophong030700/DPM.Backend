using MediatR;
using Shared.Application.Common.Interfaces;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class CreateStepCommandHandler : IRequestHandler<CreateStepCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public CreateStepCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(CreateStepCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Data;
            var userId = _currentUserService.UserId;

            var step = WorkflowStepDefine.Create(
                id: dto.Id,
                versionId: dto.VersionId,
                label: dto.Label,
                stepType: dto.StepType,
                statusCode: dto.StatusCode,
                assignRule: dto.AssignRule,
                assignValueJson: dto.AssignValueJson,
                slaTime: dto.SlaTime,
                slaUnit: dto.SlaUnit,
                positionX: dto.PositionX,
                positionY: dto.PositionY,
                isSignatureStep: dto.IsSignatureStep,
                createdBy: userId
            );

            await _repository.SaveStepAsync(step);
            return true;
        }
    }
}
