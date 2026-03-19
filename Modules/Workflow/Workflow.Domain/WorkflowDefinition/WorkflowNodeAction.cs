using Shared.Domain;

namespace Workflow.Domain.WorkflowNodes;

public class WorkflowNodeAction : Entity
{
    private int _id;
    private string _nodeId;
    private string _buttonKey;
    private string _label;
    private string? _targetNodeId;
    private string? _notifyTemplate;
    private int _sortOrder;
    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;
    private bool _isDeleted;

    public int Id => _id;
    public string NodeId => _nodeId;

    /// <summary>Key chuẩn: approve | reject | request_adjust | cancel</summary>
    public string ButtonKey => _buttonKey;
    public string Label => _label;

    /// <summary>NodeId đích khi người dùng bấm nút này. Null = kết thúc flow.</summary>
    public string? TargetNodeId => _targetNodeId;
    public string? NotifyTemplate => _notifyTemplate;
    public int SortOrder => _sortOrder;
    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;
    public bool IsDeleted => _isDeleted;

    private WorkflowNodeAction() { }

    internal static WorkflowNodeAction Create(
        string nodeId,
        string buttonKey,
        string label,
        string? targetNodeId,
        string? notifyTemplate,
        int createdBy,
        int sortOrder = 0)
    {
        return new WorkflowNodeAction
        {
            _nodeId = nodeId ?? throw new ArgumentNullException(nameof(nodeId)),
            _buttonKey = buttonKey ?? throw new ArgumentNullException(nameof(buttonKey)),
            _label = label ?? throw new ArgumentNullException(nameof(label)),
            _targetNodeId = targetNodeId,
            _notifyTemplate = notifyTemplate,
            _sortOrder = sortOrder,
            _createdAt = DateTime.UtcNow,
            _modifiedAt = DateTime.UtcNow,
            _createdBy = createdBy,
            _modifiedBy = createdBy,
            _isDeleted = false
        };
    }

    public void Update(
        string? label,
        string? targetNodeId,
        string? notifyTemplate,
        int? sortOrder,
        int modifiedBy)
    {
        _label = label ?? _label;
        _targetNodeId = targetNodeId ?? _targetNodeId;
        _notifyTemplate = notifyTemplate ?? _notifyTemplate;
        _sortOrder = sortOrder ?? _sortOrder;
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
