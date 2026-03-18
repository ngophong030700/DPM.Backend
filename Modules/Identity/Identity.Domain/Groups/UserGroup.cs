using Shared.Domain;

namespace Identity.Domain.Groups;

public class UserGroup : Entity
{
    private int _userId;
    private Users.User? _user;
    private int _groupId;
    private Group? _group;

    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;

    public int UserId => _userId;
    public Users.User? User => _user;
    public int GroupId => _groupId;
    public Group? Group => _group;

    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;

    private UserGroup() { }

    public static UserGroup Create(int userId, int groupId, int createdBy)
    {
        return new UserGroup
        {
            _userId = userId,
            _groupId = groupId,
            _createdAt = DateTime.UtcNow,
            _modifiedAt = DateTime.UtcNow,
            _createdBy = createdBy,
            _modifiedBy = createdBy
        };
    }

    public void Update(int userId, int groupId, int modifiedBy)
    {
        _userId = userId;
        _groupId = groupId;

        _modifiedBy = modifiedBy;
        _modifiedAt = DateTime.UtcNow;
    }
}
