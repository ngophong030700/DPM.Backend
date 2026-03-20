using Shared.Domain;

namespace Workflow.Domain.WorkflowDefinitions;

public enum FieldPermissionType
{
    Hidden = 0,     // Ẩn hoàn toàn
    ReadOnly = 1,   // Hiển thị nhưng không cho sửa
    Editable = 2    // Cho phép sửa
}

public class WorkflowStepFieldPermission : Entity
{
    private int _id;
    private string _stepId;
    private int _fieldId;
    private FieldPermissionType _permission;
    private bool _isRequired;

    public int Id => _id;
    public string StepId => _stepId;
    public int FieldId => _fieldId;
    public FieldPermissionType Permission => _permission;
    public bool IsRequired => _isRequired;

    private WorkflowStepFieldPermission() { }

    internal static WorkflowStepFieldPermission Create(
        string stepId,
        int fieldId,
        FieldPermissionType permission,
        bool isRequired)
    {
        return new WorkflowStepFieldPermission
        {
            _stepId = stepId ?? throw new ArgumentNullException(nameof(stepId)),
            _fieldId = fieldId,
            _permission = permission,
            _isRequired = isRequired
        };
    }

    public void Update(FieldPermissionType permission, bool isRequired)
    {
        _permission = permission;
        _isRequired = isRequired;
    }
}
