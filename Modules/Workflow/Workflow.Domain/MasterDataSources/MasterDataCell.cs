using Shared.Domain;

namespace Workflow.Domain.MasterDataSources;

public class MasterDataCell : Entity
{
    private int _id;
    private int _valueId;
    private int _columnId;
    private string? _cellValue;
    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;

    public int Id => _id;
    public int ValueId => _valueId;
    public int ColumnId => _columnId;
    public string? CellValue => _cellValue;
    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;

    private MasterDataCell() { }

    public static MasterDataCell Create(
        int valueId,
        int columnId,
        string? cellValue,
        int createdBy)
    {
        return new MasterDataCell
        {
            _valueId = valueId,
            _columnId = columnId,
            _cellValue = cellValue,
            _createdAt = DateTime.UtcNow,
            _modifiedAt = DateTime.UtcNow,
            _createdBy = createdBy,
            _modifiedBy = createdBy
        };
    }

    public void Update(
        string? cellValue,
        int modifiedBy)
    {
        _cellValue = cellValue;
        _modifiedBy = modifiedBy;
        _modifiedAt = DateTime.UtcNow;
    }
}