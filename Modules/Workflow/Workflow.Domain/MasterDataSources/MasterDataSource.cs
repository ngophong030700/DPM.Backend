using Shared.Domain;

namespace Workflow.Domain.MasterDataSources;

public class MasterDataSource : Entity, IAggregateRoot
{
    private int _id;
    private string _name;
    private string _code;
    private string? _description;
    private bool _isActive;
    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;
    private bool _isDeleted;

    private readonly List<MasterDataColumn> _columns = new();
    private readonly List<MasterDataValue> _values = new();

    public int Id => _id;
    public string Name => _name;
    public string Code => _code;
    public string? Description => _description;
    public bool IsActive => _isActive;
    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;
    public bool IsDeleted => _isDeleted;

    public IReadOnlyCollection<MasterDataColumn> Columns => _columns.AsReadOnly();
    public IReadOnlyCollection<MasterDataValue> Values => _values.AsReadOnly();

    private MasterDataSource() { }

    public void AddColumn(MasterDataColumn column)
    {
        _columns.Add(column);
    }

    public static MasterDataSource Create(
        string name,
        string code,
        string? description,
        int createdBy)
    {
        return new MasterDataSource
        {
            _name = name ?? throw new ArgumentNullException(nameof(name)),
            _code = code ?? throw new ArgumentNullException(nameof(code)),
            _description = description,
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
        string? code,
        string? description,
        bool? isActive,
        int modifiedBy)
    {
        _name = name ?? _name;
        _code = code ?? _code;
        _description = description ?? _description;
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