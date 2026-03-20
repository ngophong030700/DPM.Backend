using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;

namespace Workflow.Application.WorkflowDefinitions.Commands.Configurations
{
    public class SetupWorkflowStepsCommandHandler : IRequestHandler<SetupWorkflowStepsCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public SetupWorkflowStepsCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(SetupWorkflowStepsCommand request, CancellationToken cancellationToken)
        {
            var version = await _repository.GetVersionByIdAsync(request.VersionId);
            if (version == null)
            {
                throw new NotFoundException("Không tìm thấy phiên bản quy trình.");
            }

            var userId = _currentUserService.UserId;
            var newSteps = new List<WorkflowStepDefine>();

            foreach (var stepDto in request.Data.Steps)
            {
                var step = WorkflowStepDefine.Create(
                    id: stepDto.Id,
                    versionId: request.VersionId,
                    label: stepDto.Label,
                    stepType: stepDto.StepType,
                    statusCode: stepDto.StatusCode,
                    assignRule: stepDto.AssignRule,
                    assignValueJson: stepDto.AssignValueJson,
                    slaTime: stepDto.SlaTime,
                    slaUnit: stepDto.SlaUnit,
                    positionX: stepDto.PositionX,
                    positionY: stepDto.PositionY,
                    isSignatureStep: stepDto.IsSignatureStep,
                    createdBy: userId
                );

                foreach (var actionDto in stepDto.Actions)
                {
                    var action = step.AddAction(
                        buttonKey: actionDto.ButtonKey,
                        label: actionDto.Label,
                        targetStepId: actionDto.TargetStepId,
                        notifyTemplate: actionDto.NotifyTemplate,
                        createdBy: userId
                    );

                    foreach (var ruleDto in actionDto.Rules)
                    {
                        action.AddRule(ruleDto.ConditionExpression, ruleDto.TargetStepId, ruleDto.SortOrder);
                    }
                }

                foreach (var docDto in stepDto.Documents)
                {
                    step.AddDocument(
                        docTypeName: docDto.DocTypeName,
                        isRequired: docDto.IsRequired,
                        checkDigitalSignature: docDto.CheckDigitalSignature,
                        createdBy: userId
                    );
                }

                foreach (var permDto in stepDto.FieldPermissions)
                {
                    step.SetFieldPermission(permDto.FieldId, permDto.Permission, permDto.IsRequired);
                }

                foreach (var hookDto in stepDto.Hooks)
                {
                    step.AddHook(hookDto.EventType, hookDto.ActionType, hookDto.ConfigJson, hookDto.SortOrder);
                }

                newSteps.Add(step);
            }

            await _repository.SaveStepsAsync(request.VersionId, newSteps);

            return true;
        }
    }
}