using Shared.Domain;

namespace Workflow.Domain.WorkflowNodes;

public class WorkflowNode : Entity, IAggregateRoot
{
    private string _id;         // ID từ ReactFlow (VD: "node_1")
    private int _versionId;
    private string _label;
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
    private bool _isSignatureStep;
    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;
    private bool _isDeleted;

    private readonly List<WorkflowNodeAction> _actions = new();
    private readonly List<WorkflowNodeDocument> _documents = new();

    public string Id => _id;
    public int VersionId => _versionId;
    public string Label => _label;
    public AssignRule AssignRule => _assignRule;
    public string? AssignValueJson => _assignValueJson;
    public int? SlaTime => _slaTime;
    public SlaUnit? SlaUnit => _slaUnit;
    public bool IsSignatureStep => _isSignatureStep;
    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;
    public bool IsDeleted => _isDeleted;

    public IReadOnlyCollection<WorkflowNodeAction> Actions => _actions.AsReadOnly();
    public IReadOnlyCollection<WorkflowNodeDocument> Documents => _documents.AsReadOnly();

    private WorkflowNode() { }

    public static WorkflowNode Create(
        string id,
        int versionId,
        string label,
        AssignRule assignRule,
        string? assignValueJson,
        int? slaTime,
        SlaUnit? slaUnit,
        bool isSignatureStep,
        int createdBy)
    {
        return new WorkflowNode
        {
            _id = id ?? throw new ArgumentNullException(nameof(id)),
            _versionId = versionId,
            _label = label ?? throw new ArgumentNullException(nameof(label)),
            _assignRule = assignRule,
            _assignValueJson = assignValueJson,
            _slaTime = slaTime,
            _slaUnit = slaUnit,
            _isSignatureStep = isSignatureStep,
            _createdAt = DateTime.UtcNow,
            _modifiedAt = DateTime.UtcNow,
            _createdBy = createdBy,
            _modifiedBy = createdBy,
            _isDeleted = false
        };
    }

    public WorkflowNodeAction AddAction(
        string buttonKey,
        string label,
        string? targetNodeId,
        string? notifyTemplate,
        int createdBy)
    {
        if (_actions.Any(a => a.ButtonKey == buttonKey))
            throw new InvalidOperationException($"Action '{buttonKey}' already exists on this node.");

        var action = WorkflowNodeAction.Create(
            nodeId: _id,
            buttonKey: buttonKey,
            label: label,
            targetNodeId: targetNodeId,
            notifyTemplate: notifyTemplate,
            createdBy: createdBy);

        _actions.Add(action);
        return action;
    }

    public WorkflowNodeDocument AddDocument(
        string docTypeName,
        bool isRequired,
        bool checkDigitalSignature,
        int createdBy)
    {
        var doc = WorkflowNodeDocument.Create(
            nodeId: _id,
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

    public void SoftDelete(int modifiedBy)
    {
        if (_isDeleted) return;
        _isDeleted = true;
        _modifiedBy = modifiedBy;
        _modifiedAt = DateTime.UtcNow;
    }
}
