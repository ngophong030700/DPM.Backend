using Shared.Domain;

namespace Workflow.Domain.WorkflowFields;

public class WorkflowGridColumn : Entity
{
    private int _id;
    private int _parentFieldId;
    private string _name;
    private string _label;
    private FieldDataType _dataType;
    private DataSourceType? _dataSourceType;
    private string? _dataSourceConfigJson;
    private string? _settingsJson;
    private int _sortOrder;
    private bool _isRequired;
    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;
    private bool _isDeleted;

    public int Id => _id;
    public int ParentFieldId => _parentFieldId;
    public string Name => _name;
    public string Label => _label;
    public FieldDataType DataType => _dataType;
    public DataSourceType? DataSourceType => _dataSourceType;
    public string? DataSourceConfigJson => _dataSourceConfigJson;
    public string? SettingsJson => _settingsJson;
    public int SortOrder => _sortOrder;
    public bool IsRequired => _isRequired;
    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;
    public bool IsDeleted => _isDeleted;

    private WorkflowGridColumn() { }

    internal static WorkflowGridColumn Create(
        int parentFieldId,
        string name,
        string label,
        FieldDataType dataType,
        DataSourceType? dataSourceType,
        string? dataSourceConfigJson,
        string? settingsJson,
        int sortOrder,
        bool isRequired,
        int createdBy)
    {
        return new WorkflowGridColumn
        {
            _parentFieldId = parentFieldId,
            _name = name ?? throw new ArgumentNullException(nameof(name)),
            _label = label ?? throw new ArgumentNullException(nameof(label)),
            _dataType = dataType,
            _dataSourceType = dataSourceType,
            _dataSourceConfigJson = dataSourceConfigJson,
            _settingsJson = settingsJson,
            _sortOrder = sortOrder,
            _isRequired = isRequired,
            _createdAt = DateTime.UtcNow,
            _modifiedAt = DateTime.UtcNow,
            _createdBy = createdBy,
            _modifiedBy = createdBy,
            _isDeleted = false
        };
    }

    public void Update(
        string? label,
        DataSourceType? dataSourceType,
        string? dataSourceConfigJson,
        string? settingsJson,
        int? sortOrder,
        bool? isRequired,
        int modifiedBy)
    {
        _label = label ?? _label;
        _dataSourceType = dataSourceType ?? _dataSourceType;
        _dataSourceConfigJson = dataSourceConfigJson ?? _dataSourceConfigJson;
        _settingsJson = settingsJson ?? _settingsJson;
        _sortOrder = sortOrder ?? _sortOrder;
        _isRequired = isRequired ?? _isRequired;
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
