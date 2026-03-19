using Shared.Domain;

namespace Workflow.Domain.WorkflowFields;

public class WorkflowField : Entity, IAggregateRoot
{
    private int _id;
    private int _versionId;
    private string _name;
    private string _label;
    private FieldDataType _dataType;
    private DataSourceType? _dataSourceType;
    private string? _dataSourceConfigJson;
    private string? _fieldFormula;
    private string? _settingsJson;
    private int _sortOrder;
    private bool _isRequired;
    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;
    private bool _isDeleted;

    private readonly List<WorkflowGridColumn> _gridColumns = new();

    public int Id => _id;
    public int VersionId => _versionId;
    public string Name => _name;
    public string Label => _label;
    public FieldDataType DataType => _dataType;
    public DataSourceType? DataSourceType => _dataSourceType;

    /// <summary>JSON của DataSourceConfig — serialize/deserialize ở Infrastructure layer.</summary>
    public string? DataSourceConfigJson => _dataSourceConfigJson;

    /// <summary>Công thức nếu DataType = Formula. VD: "{{qty}} * {{price}}"</summary>
    public string? FieldFormula => _fieldFormula;

    /// <summary>JSON: { decimalPlaces, dateFormat, placeholder, min, max }</summary>
    public string? SettingsJson => _settingsJson;

    public int SortOrder => _sortOrder;
    public bool IsRequired => _isRequired;
    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;
    public bool IsDeleted => _isDeleted;

    public IReadOnlyCollection<WorkflowGridColumn> GridColumns => _gridColumns.AsReadOnly();

    private WorkflowField() { }

    public static WorkflowField Create(
        int versionId,
        string name,
        string label,
        FieldDataType dataType,
        DataSourceType? dataSourceType,
        string? dataSourceConfigJson,
        string? fieldFormula,
        string? settingsJson,
        int sortOrder,
        bool isRequired,
        int createdBy)
    {
        if (dataType == FieldDataType.Formula && string.IsNullOrWhiteSpace(fieldFormula))
            throw new ArgumentException("FieldFormula is required when DataType is Formula.");

        if (dataType == FieldDataType.Grid && dataSourceType != null)
            throw new ArgumentException("Grid fields do not use DataSourceType.");

        return new WorkflowField
        {
            _versionId = versionId,
            _name = name ?? throw new ArgumentNullException(nameof(name)),
            _label = label ?? throw new ArgumentNullException(nameof(label)),
            _dataType = dataType,
            _dataSourceType = dataSourceType,
            _dataSourceConfigJson = dataSourceConfigJson,
            _fieldFormula = fieldFormula,
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

    public WorkflowGridColumn AddGridColumn(
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
        if (_dataType != FieldDataType.Grid)
            throw new InvalidOperationException("Only Grid fields can have grid columns.");

        if (dataType == FieldDataType.Grid)
            throw new InvalidOperationException("Nested Grid is not allowed.");

        var column = WorkflowGridColumn.Create(
            parentFieldId: _id,
            name: name,
            label: label,
            dataType: dataType,
            dataSourceType: dataSourceType,
            dataSourceConfigJson: dataSourceConfigJson,
            settingsJson: settingsJson,
            sortOrder: sortOrder,
            isRequired: isRequired,
            createdBy: createdBy);

        _gridColumns.Add(column);
        return column;
    }

    public void Update(
        string? label,
        DataSourceType? dataSourceType,
        string? dataSourceConfigJson,
        string? fieldFormula,
        string? settingsJson,
        int? sortOrder,
        bool? isRequired,
        int modifiedBy)
    {
        _label = label ?? _label;
        _dataSourceType = dataSourceType ?? _dataSourceType;
        _dataSourceConfigJson = dataSourceConfigJson ?? _dataSourceConfigJson;
        _fieldFormula = fieldFormula ?? _fieldFormula;
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
