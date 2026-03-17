using Shared.Domain;

namespace Identity.Domain.Positions;

public class Position : Entity
{
    private int _id;
    private string _name;
    private string? _description;

    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;
    private bool _isDeleted;

    public int Id => _id;
    public string Name => _name;
    public string? Description => _description;

    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;
    public bool IsDeleted => _isDeleted;

    private Position() { }

    public static Position Create(string name, string? description, int createdBy)
    {
        return new Position
        {
            _name = name ?? throw new ArgumentNullException(nameof(name)),
            _description = description,
            _createdAt = DateTime.UtcNow,
            _modifiedAt = DateTime.UtcNow,
            _createdBy = createdBy,
            _modifiedBy = createdBy,
            _isDeleted = false
        };
    }

    public void Update(string? name, string? description, int modifiedBy)
    {
        _name = name ?? _name;
        _description = description ?? _description;

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