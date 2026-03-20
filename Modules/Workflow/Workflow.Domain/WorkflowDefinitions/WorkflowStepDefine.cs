using Shared.Domain;

namespace Workflow.Domain.WorkflowDefinitions;

public enum WorkflowStepType
{
    Start = 0,      // Bước bắt đầu
    Task = 1,       // Bước xử lý nghiệp vụ
    End = 2         // Bước kết thúc
}

public class WorkflowStepDefine : Entity, IAggregateRoot
{
    private string _id;         // ID từ ReactFlow (VD: "node_1")
    private int _versionId;
    private string _label;
    private WorkflowStepType _stepType;
    private string? _statusCode; // Mã trạng thái nghiệp vụ (VD: "WAIT_APPROVE")
    private AssignRule _assignRule;

    /// <summary>
    /// JSON chứa danh sách User/Group ID hoặc key của field chứa người xử lý.
    /// Schema phụ thuộc vào AssignRule:
    ///   HandPickUser/Group: ["id1","id2"]
    ///   FieldBased: "fieldName"
    /// </summary>
    private string? _assignValueJson;

    private int? _slaTime;
    private SlaUnit? _slaUnit;
    private double _positionX;
    private double _positionY;
    private bool _isSignatureStep;
    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;
    private bool _isDeleted;

    private readonly List<WorkflowStepDefineAction> _actions = new();
    private readonly List<WorkflowStepDefineDocument> _documents = new();
    private readonly List<WorkflowStepFieldPermission> _fieldPermissions = new();
    private readonly List<WorkflowStepHook> _hooks = new();

    public string Id => _id;
    public int VersionId => _versionId;
    public string Label => _label;
    public WorkflowStepType StepType => _stepType;
    public string? StatusCode => _statusCode;
    public AssignRule AssignRule => _assignRule;
    public string? AssignValueJson => _assignValueJson;
    public int? SlaTime => _slaTime;
    public SlaUnit? SlaUnit => _slaUnit;
    public double PositionX => _positionX;
    public double PositionY => _positionY;
    public bool IsSignatureStep => _isSignatureStep;
    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;
    public bool IsDeleted => _isDeleted;

    public IReadOnlyCollection<WorkflowStepDefineAction> Actions => _actions.AsReadOnly();
    public IReadOnlyCollection<WorkflowStepDefineDocument> Documents => _documents.AsReadOnly();
    public IReadOnlyCollection<WorkflowStepFieldPermission> FieldPermissions => _fieldPermissions.AsReadOnly();
    public IReadOnlyCollection<WorkflowStepHook> Hooks => _hooks.AsReadOnly();

    private WorkflowStepDefine() { }

    public static WorkflowStepDefine Create(
        string id,
        int versionId,
        string label,
        WorkflowStepType stepType,
        string? statusCode,
        AssignRule assignRule,
        string? assignValueJson,
        int? slaTime,
        SlaUnit? slaUnit,
        double positionX,
        double positionY,
        bool isSignatureStep,
        int createdBy)
    {
        return new WorkflowStepDefine
        {
            _id = id ?? throw new ArgumentNullException(nameof(id)),
            _versionId = versionId,
            _label = label ?? throw new ArgumentNullException(nameof(label)),
            _stepType = stepType,
            _statusCode = statusCode,
            _assignRule = assignRule,
            _assignValueJson = assignValueJson,
            _slaTime = slaTime,
            _slaUnit = slaUnit,
            _positionX = positionX,
            _positionY = positionY,
            _isSignatureStep = isSignatureStep,
            _createdAt = DateTime.UtcNow,
            _modifiedAt = DateTime.UtcNow,
            _createdBy = createdBy,
            _modifiedBy = createdBy,
            _isDeleted = false
        };
    }

    public WorkflowStepFieldPermission SetFieldPermission(int fieldId, FieldPermissionType permission, bool isRequired)
    {
        var existing = _fieldPermissions.FirstOrDefault(p => p.FieldId == fieldId);
        if (existing != null)
        {
            existing.Update(permission, isRequired);
            return existing;
        }

        var newPerm = WorkflowStepFieldPermission.Create(_id, fieldId, permission, isRequired);
        _fieldPermissions.Add(newPerm);
        return newPerm;
    }

    public WorkflowStepHook AddHook(HookEventType eventType, HookActionType actionType, string configJson, int sortOrder)
    {
        var hook = WorkflowStepHook.Create(_id, eventType, actionType, configJson, sortOrder);
        _hooks.Add(hook);
        return hook;
    }

    public WorkflowStepDefineAction AddAction(
        string buttonKey,
        string label,
        string? targetStepId,
        string? notifyTemplate,
        int createdBy)
    {
        if (_actions.Any(a => a.ButtonKey == buttonKey))
            throw new InvalidOperationException($"Action '{buttonKey}' already exists on this node.");

        var action = WorkflowStepDefineAction.Create(
            stepId: _id,
            buttonKey: buttonKey,
            label: label,
            targetStepId: targetStepId,
            notifyTemplate: notifyTemplate,
            createdBy: createdBy);

        _actions.Add(action);
        return action;
    }

    public WorkflowStepDefineDocument AddDocument(
        string docTypeName,
        bool isRequired,
        bool checkDigitalSignature,
        int createdBy)
    {
        var doc = WorkflowStepDefineDocument.Create(
            stepId: _id,
            docTypeName: docTypeName,
            isRequired: isRequired,
            checkDigitalSignature: checkDigitalSignature,
            createdBy: createdBy);

        _documents.Add(doc);
        return doc;
    }

    public void Update(
        string? label,
        AssignRule? assignRule,
        string? assignValueJson,
        int? slaTime,
        SlaUnit? slaUnit,
        bool? isSignatureStep,
        int modifiedBy)
    {
        _label = label ?? _label;
        _assignRule = assignRule ?? _assignRule;
        _assignValueJson = assignValueJson ?? _assignValueJson;
        _slaTime = slaTime ?? _slaTime;
        _slaUnit = slaUnit ?? _slaUnit;
        _isSignatureStep = isSignatureStep ?? _isSignatureStep;
        _modifiedBy = modifiedBy;
        _modifiedAt = DateTime.UtcNow;
    }

    public void UpdatePosition(double x, double y, int modifiedBy)
    {
        _positionX = x;
        _positionY = y;
        _modifiedBy = modifiedBy;
        _modifiedAt = DateTime.UtcNow;
    }

    public void SoftDelete(int modifiedBy)
    {
        if (_isDeleted) return;
        _isDeleted = true;
        _modifiedBy = modifiedBy;
        _modifiedAt = DateTime.UtcNow;
    }
}
