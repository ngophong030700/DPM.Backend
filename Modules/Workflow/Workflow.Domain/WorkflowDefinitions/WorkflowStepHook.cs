using Shared.Domain;

namespace Workflow.Domain.WorkflowDefinitions;

public enum HookEventType
{
    OnStepEntry = 0,    // Khi đơn bắt đầu chuyển vào bước này
    OnStepExit = 1,     // Khi đơn chuyển ra khỏi bước này
    OnActionExecute = 2 // Khi một action cụ thể được thực hiện
}

public enum HookActionType
{
    SendEmail = 0,      // Gửi email thông báo
    CallWebhook = 1,    // Gọi API ngoại vi
    UpdateFieldValue = 2, // Tự động cập nhật giá trị của một field khác
    CustomTask = 3      // Tác vụ code tùy chỉnh
}

public class WorkflowStepHook : Entity
{
    private int _id;
    private string _stepId;
    private HookEventType _eventType;
    private HookActionType _actionType;

    /// <summary>
    /// JSON cấu hình chi tiết (VD: API URL, Email Template ID, Logic cập nhật field).
    /// </summary>
    private string _configJson;
    private int _sortOrder;

    public int Id => _id;
    public string StepId => _stepId;
    public HookEventType EventType => _eventType;
    public HookActionType ActionType => _actionType;
    public string ConfigJson => _configJson;
    public int SortOrder => _sortOrder;

    private WorkflowStepHook() { }

    internal static WorkflowStepHook Create(
        string stepId,
        HookEventType eventType,
        HookActionType actionType,
        string configJson,
        int sortOrder)
    {
        return new WorkflowStepHook
        {
            _stepId = stepId ?? throw new ArgumentNullException(nameof(stepId)),
            _eventType = eventType,
            _actionType = actionType,
            _configJson = configJson ?? throw new ArgumentNullException(nameof(configJson)),
            _sortOrder = sortOrder
        };
    }

    public void Update(HookEventType eventType, HookActionType actionType, string configJson, int sortOrder)
    {
        _eventType = eventType;
        _actionType = actionType;
        _configJson = configJson ?? _configJson;
        _sortOrder = sortOrder;
    }
}
