using Shared.Domain;

namespace Workflow.Domain.WorkflowDefinitions;

public enum ChartType
{
    None = 0,
    Pie = 1,
    Bar = 2,
    Line = 3
}

public class WorkflowReport : Entity, IAggregateRoot
{
    private int _id;
    private int _versionId;
    private string _name;

    /// <summary>
    /// JSON: danh sách các fieldId / gridColumnId được kéo vào báo cáo, kèm cấu hình hiển thị.
    /// VD: [{ "fieldId": 1, "label": "Tên", "sortOrder": 0 }]
    /// </summary>
    private string? _fieldsConfigJson;

    /// <summary>
    /// JSON: { type: 'Pie|Bar|Line', xAxis: fieldId, yAxis: fieldId }
    /// Null nếu báo cáo không có chart.
    /// </summary>
    private string? _chartConfigJson;

    private bool _isActive;
    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;
    private bool _isDeleted;

    public int Id => _id;
    public int VersionId => _versionId;
    public string Name => _name;
    public string? FieldsConfigJson => _fieldsConfigJson;
    public string? ChartConfigJson => _chartConfigJson;
    public bool IsActive => _isActive;
    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;
    public bool IsDeleted => _isDeleted;

    private WorkflowReport() { }

    public static WorkflowReport Create(
        int versionId,
        string name,
        string? fieldsConfigJson,
        string? chartConfigJson,
        int createdBy)
    {
        return new WorkflowReport
        {
            _versionId = versionId,
            _name = name ?? throw new ArgumentNullException(nameof(name)),
            _fieldsConfigJson = fieldsConfigJson,
            _chartConfigJson = chartConfigJson,
            _isActive = true,
            _createdAt = DateTime.UtcNow,
            _modifiedAt = DateTime.UtcNow,
            _createdBy = createdBy,
            _modifiedBy = createdBy,
            _isDeleted = false
        };
    }

    public void Update(
        string? name,
        string? fieldsConfigJson,
        string? chartConfigJson,
        bool? isActive,
        int modifiedBy)
    {
        _name = name ?? _name;
        _fieldsConfigJson = fieldsConfigJson ?? _fieldsConfigJson;
        _chartConfigJson = chartConfigJson ?? _chartConfigJson;
        _isActive = isActive ?? _isActive;
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
