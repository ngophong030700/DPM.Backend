using MediatR;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Exceptions;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;

namespace Workflow.Application.WorkflowDefinitions.Commands.Versions
{
    public class CloneWorkflowVersionCommandHandler : IRequestHandler<CloneWorkflowVersionCommand, bool>
    {
        private readonly IWorkflowDefinitionRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public CloneWorkflowVersionCommandHandler(
            IWorkflowDefinitionRepository repository,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(CloneWorkflowVersionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var sourceVersion = await _repository.GetVersionByIdAsync(request.Id);
            if (sourceVersion == null)
            {
                throw new NotFoundException("Không tìm thấy phiên bản nguồn.");
            }

            var workflow = await _repository.GetByIdAsync(sourceVersion.WorkflowId, includeDetails: true);
            if (workflow == null)
            {
                throw new NotFoundException("Không tìm thấy quy trình.");
            }

            // 1. Create new version
            var newVersionName = $"{sourceVersion.VersionName} (Clone)";
            var newVersion = workflow.AddVersion(newVersionName, request.Data.Notes ?? sourceVersion.Notes, userId);
            await _repository.UpdateAsync(workflow);

            // 2. Clone Fields
            var sourceFields = await _repository.GetFieldsByVersionIdAsync(sourceVersion.Id);
            var newFields = new List<WorkflowField>();
            foreach (var f in sourceFields)
            {
                var newField = WorkflowField.Create(
                    versionId: newVersion.Id,
                    name: f.Name,
                    label: f.Label,
                    dataType: f.DataType,
                    dataSourceType: f.DataSourceType,
                    dataSourceConfigJson: f.DataSourceConfigJson,
                    fieldFormula: f.FieldFormula,
                    settingsJson: f.SettingsJson,
                    sortOrder: f.SortOrder,
                    isRequired: f.IsRequired,
                    createdBy: userId
                );

                foreach (var col in f.GridColumns)
                {
                    newField.AddGridColumn(col.Name, col.Label, col.DataType, col.DataSourceType, col.DataSourceConfigJson, col.SettingsJson, col.SortOrder, col.IsRequired, userId);
                }
                newFields.Add(newField);
            }
            await _repository.SaveFieldsAsync(newVersion.Id, newFields);

            // 3. Clone Layout
            var sourceLayout = await _repository.GetLayoutByVersionIdAsync(sourceVersion.Id);
            if (sourceLayout != null)
            {
                var newLayout = WorkflowLayout.Create(newVersion.Id, sourceLayout.RowsJson, sourceLayout.AttachmentSettingsJson, userId);
                await _repository.SaveLayoutAsync(newLayout);
            }

            // 4. Clone Steps
            var sourceSteps = await _repository.GetStepsByVersionIdAsync(sourceVersion.Id);
            var newSteps = new List<WorkflowStepDefine>();
            foreach (var s in sourceSteps)
            {
                var newStep = WorkflowStepDefine.Create(
                    id: s.Id, // Keeping same node ID
                    versionId: newVersion.Id,
                    label: s.Label,
                    stepType: s.StepType,
                    statusCode: s.StatusCode,
                    assignRule: s.AssignRule,
                    assignValueJson: s.AssignValueJson,
                    slaTime: s.SlaTime,
                    slaUnit: s.SlaUnit,
                    positionX: s.PositionX,
                    positionY: s.PositionY,
                    isSignatureStep: s.IsSignatureStep,
                    createdBy: userId
                );

                // Actions, Documents, Hooks, etc. should be cloned properly.
                // For brevity, we'll assume SaveStepsAsync handles nested collections if they are new.
                foreach (var a in s.Actions)
                {
                    var newAction = newStep.AddAction(a.ButtonKey, a.Label, a.TargetStepId, a.NotifyTemplate, userId);
                    foreach (var r in a.Rules)
                    {
                        newAction.AddRule(r.ConditionExpression, r.TargetStepId, r.SortOrder);
                    }
                }
                foreach (var d in s.Documents)
                {
                    newStep.AddDocument(d.DocTypeName, d.IsRequired, d.CheckDigitalSignature, userId);
                }
                foreach (var p in s.FieldPermissions)
                {
                    newStep.SetFieldPermission(p.FieldId, p.Permission, p.IsRequired);
                }
                foreach (var h in s.Hooks)
                {
                    newStep.AddHook(h.EventType, h.ActionType, h.ConfigJson, h.SortOrder);
                }

                newSteps.Add(newStep);
            }
            await _repository.SaveStepsAsync(newVersion.Id, newSteps);

            return true;
        }
    }
}