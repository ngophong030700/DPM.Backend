using Shared.Domain;

namespace Workflow.Domain.WorkflowDefinitions;

/// <summary>
/// Định nghĩa logic rẽ nhánh cho một Action.
/// Nếu ConditionExpression thỏa mãn, đơn sẽ đi đến TargetStepId này.
/// </summary>
public class WorkflowActionRule : Entity
{
    private int _id;
    private int _actionId;

    /// <summary>
    /// Biểu thức logic (VD: "{{amount}} > 10000").
    /// Nếu để trống, coi như là luật mặc định.
    /// </summary>
    private string? _conditionExpression;
    private string _targetStepId;
    private int _sortOrder;

    public int Id => _id;
    public int ActionId => _actionId;
    public string? ConditionExpression => _conditionExpression;
    public string TargetStepId => _targetStepId;
    public int SortOrder => _sortOrder;

    private WorkflowActionRule() { }

    internal static WorkflowActionRule Create(
        int actionId,
        string? conditionExpression,
        string targetStepId,
        int sortOrder)
    {
        return new WorkflowActionRule
        {
            _actionId = actionId,
            _conditionExpression = conditionExpression,
            _targetStepId = targetStepId ?? throw new ArgumentNullException(nameof(targetStepId)),
            _sortOrder = sortOrder
        };
    }

    public void Update(string? conditionExpression, string targetStepId, int sortOrder)
    {
        _conditionExpression = conditionExpression;
        _targetStepId = targetStepId ?? _targetStepId;
        _sortOrder = sortOrder;
    }
}
