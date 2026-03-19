using Shared.Domain;

namespace Workflow.Domain.MasterDataSources;

public class MasterDataValue : Entity
{
    private int _id;
    private int _sourceId;
    private string _displayName;
    private string _valueCode;
    private int _sortOrder;
    private bool _isActive;
    private bool _isDeleted;
    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;

    private readonly List<MasterDataCell> _cells = new();

    public int Id => _id;
    public int SourceId => _sourceId;
    public string DisplayName => _displayName;
    public string ValueCode => _valueCode;
    public int SortOrder => _sortOrder;
    public bool IsActive => _isActive;
    public bool IsDeleted => _isDeleted;
    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;

    public IReadOnlyCollection<MasterDataCell> Cells => _cells.AsReadOnly();

    private MasterDataValue() { }

    public void AddCell(MasterDataCell cell)
    {
        _cells.Add(cell);
    }

    public void UpdateOrAddCell(int columnId, string? cellValue, int modifiedBy)
    {
        var cell = _cells.FirstOrDefault(c => c.ColumnId == columnId);
        if (cell != null)
        {
            cell.Update(cellValue, modifiedBy);
        }
        else
        {
            AddCell(MasterDataCell.Create(Id, columnId, cellValue, modifiedBy));
        }
    }

    public static MasterDataValue Create(
        int sourceId,
        string displayName,
        string valueCode,
        int sortOrder,
        int createdBy)
    {
        return new MasterDataValue
        {
            _sourceId = sourceId,
            _displayName = displayName ?? throw new ArgumentNullException(nameof(displayName)),
            _valueCode = valueCode ?? throw new ArgumentNullException(nameof(valueCode)),
            _sortOrder = sortOrder,
            _isActive = true,
            _createdAt = DateTime.UtcNow,
            _modifiedAt = DateTime.UtcNow,
            _createdBy = createdBy,
            _modifiedBy = createdBy,
            _isDeleted = false
        };
    }

    public void Update(
        string? displayName,
        string? valueCode,
        int? sortOrder,
        bool? isActive,
        int modifiedBy)
    {
        _displayName = displayName ?? _displayName;
        _valueCode = valueCode ?? _valueCode;
        _sortOrder = sortOrder ?? _sortOrder;
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