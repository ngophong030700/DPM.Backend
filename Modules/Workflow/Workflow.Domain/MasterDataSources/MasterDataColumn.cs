using Shared.Domain;

namespace Workflow.Domain.MasterDataSources;

public class MasterDataColumn : Entity
{
    private int _id;
    private int _sourceId;
    private string _columnKey;
    private string _columnLabel;
    private string _dataType;
    private bool _isRequired;
    private int _sortOrder;
    private bool _isDeleted;
    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;

    public int Id => _id;
    public int SourceId => _sourceId;
    public string ColumnKey => _columnKey;
    public string ColumnLabel => _columnLabel;
    public string DataType => _dataType;
    public bool IsRequired => _isRequired;
    public int SortOrder => _sortOrder;
    public bool IsDeleted => _isDeleted;
    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;

    private MasterDataColumn() { }

    public static MasterDataColumn Create(
        int sourceId,
        string columnKey,
        string columnLabel,
        string dataType,
        bool isRequired,
        int sortOrder,
        int createdBy)
    {
        return new MasterDataColumn
        {
            _sourceId = sourceId,
            _columnKey = columnKey ?? throw new ArgumentNullException(nameof(columnKey)),
            _columnLabel = columnLabel ?? throw new ArgumentNullException(nameof(columnLabel)),
            _dataType = dataType ?? throw new ArgumentNullException(nameof(dataType)),
            _isRequired = isRequired,
            _sortOrder = sortOrder,
            _createdAt = DateTime.UtcNow,
            _modifiedAt = DateTime.UtcNow,
            _createdBy = createdBy,
            _modifiedBy = createdBy,
            _isDeleted = false
        };
    }

    public void Update(
        string? columnKey,
        string? columnLabel,
        string? dataType,
        bool? isRequired,
        int? sortOrder,
        int modifiedBy)
    {
        _columnKey = columnKey ?? _columnKey;
        _columnLabel = columnLabel ?? _columnLabel;
        _dataType = dataType ?? _dataType;
        _isRequired = isRequired ?? _isRequired;
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