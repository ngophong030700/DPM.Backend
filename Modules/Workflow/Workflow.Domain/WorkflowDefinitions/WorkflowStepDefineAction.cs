using Shared.Domain;

namespace Workflow.Domain.WorkflowDefinitions;

public class WorkflowStepDefineAction : Entity
{
    private int _id;
    private string _stepId;
    private string _buttonKey;
    private string _label;
    private string? _targetStepId;
    private string? _notifyTemplate;
    private int _sortOrder;
    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;
    private bool _isDeleted;

    public int Id => _id;
    public string StepId => _stepId;

    /// <summary>Key chuẩn: approve | reject | request_adjust | cancel</summary>
    public string ButtonKey => _buttonKey;
    public string Label => _label;

    /// <summary>StepId đích khi người dùng bấm nút này. Null = kết thúc flow.</summary>
    public string? TargetStepId => _targetStepId;
    public string? NotifyTemplate => _notifyTemplate;
    public int SortOrder => _sortOrder;
    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;
    public bool IsDeleted => _isDeleted;

    private readonly List<WorkflowActionRule> _rules = new();
    public IReadOnlyCollection<WorkflowActionRule> Rules => _rules.AsReadOnly();

    private WorkflowStepDefineAction() { }

    internal static WorkflowStepDefineAction Create(
        string stepId,
        string buttonKey,
        string label,
        string? targetStepId,
        string? notifyTemplate,
        int createdBy,
        int sortOrder = 0)
    {
        return new WorkflowStepDefineAction
        {
            _stepId = stepId ?? throw new ArgumentNullException(nameof(stepId)),
            _buttonKey = buttonKey ?? throw new ArgumentNullException(nameof(buttonKey)),
            _label = label ?? throw new ArgumentNullException(nameof(label)),
            _targetStepId = targetStepId,
            _notifyTemplate = notifyTemplate,
            _sortOrder = sortOrder,
            _createdAt = DateTime.UtcNow,
            _modifiedAt = DateTime.UtcNow,
            _createdBy = createdBy,
            _modifiedBy = createdBy,
            _isDeleted = false
        };
    }

    public WorkflowActionRule AddRule(string? conditionExpression, string targetStepId, int sortOrder)
    {
        var rule = WorkflowActionRule.Create(_id, conditionExpression, targetStepId, sortOrder);
        _rules.Add(rule);
        return rule;
    }

    public void Update(
        string? label,
        string? targetStepId,
        string? notifyTemplate,
        int? sortOrder,
        int modifiedBy)
    {
        _label = label ?? _label;
        _targetStepId = targetStepId ?? _targetStepId;
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
