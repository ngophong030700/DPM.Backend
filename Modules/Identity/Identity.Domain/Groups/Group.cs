using Shared.Domain;

namespace Identity.Domain.Groups;

public class Group : Entity
{
    private int _id;
    private string _name;
    private string? _description;

    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;
    private bool _isDeleted;

    private readonly List<UserGroup> _userGroups = new();

    public int Id => _id;
    public string Name => _name;
    public string? Description => _description;

    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;
    public bool IsDeleted => _isDeleted;

    public IReadOnlyCollection<UserGroup> UserGroups => _userGroups.AsReadOnly();

    private Group() { }

    public static Group Create(string name, string? description, int createdBy)
    {
        return new Group
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

    public void AddUserGroup(UserGroup userGroup)
    {
        if (_userGroups.Any(x => x.UserId == userGroup.UserId)) return;

        _userGroups.Add(userGroup);
        _modifiedAt = DateTime.UtcNow;
    }

    public void RemoveUserGroup(int userId)
    {
        var entity = _userGroups.FirstOrDefault(x => x.UserId == userId);
        if (entity != null)
        {
            _userGroups.Remove(entity);
            _modifiedAt = DateTime.UtcNow;
        }
    }
}