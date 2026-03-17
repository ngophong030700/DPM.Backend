using Shared.Domain;

namespace Identity.Domain.Departments;

public class Department : Entity
{
    private int _id;
    private string _name;
    private string? _description;

    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;
    private bool _isDeleted;

    // 🌳 Tree
    private int _index;
    private int _level;
    private string _pathCode;
    private int? _parentId;
    private Department? _parent;

    private readonly List<Department> _childrens = new();

    public int Id => _id;
    public string Name => _name;
    public string? Description => _description;

    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;
    public bool IsDeleted => _isDeleted;

    public int Index => _index;
    public int Level => _level;
    public string PathCode => _pathCode;
    public int? ParentId => _parentId;
    public Department? Parent => _parent;

    public IReadOnlyCollection<Department> Childrens => _childrens;

    private Department() { }

    public static Department Create(
        string name,
        string? description,
        int index,
        int level,
        string pathCode,
        int createdBy,
        int? parentId = null)
    {
        return new Department
        {
            _name = name ?? throw new ArgumentNullException(nameof(name)),
            _description = description,
            _index = index,
            _level = level,
            _pathCode = pathCode ?? throw new ArgumentNullException(nameof(pathCode)),
            _parentId = parentId,

            _createdAt = DateTime.UtcNow,
            _modifiedAt = DateTime.UtcNow,
            _createdBy = createdBy,
            _modifiedBy = createdBy,
            _isDeleted = false
        };
    }

    public void Update(
        string? name,
        string? description,
        int modifiedBy,
        int? index = null,
        int? level = null,
        string? pathCode = null,
        int? parentId = null)
    {
        _name = name ?? _name;
        _description = description ?? _description;

        _index = index ?? _index;
        _level = level ?? _level;
        _pathCode = pathCode ?? _pathCode;
        _parentId = parentId ?? _parentId;

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