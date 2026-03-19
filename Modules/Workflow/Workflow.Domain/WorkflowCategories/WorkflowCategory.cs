using Shared.Domain;

namespace Workflow.Domain.WorkflowCategories;

public class WorkflowCategory : Entity, IAggregateRoot
{
    private int _id;
    private string _name;
    private string? _description;
    private string _icon;

    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;
    private bool _isDeleted;

    public int Id => _id;
    public string Name => _name;
    public string? Description => _description;
    public string Icon => _icon;

    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;
    public bool IsDeleted => _isDeleted;

    private WorkflowCategory() { }

    public static WorkflowCategory Create(
        string name,
        string? description,
        string icon,
        int createdBy)
    {
        return new WorkflowCategory
        {
            _name = name ?? throw new ArgumentNullException(nameof(name)),
            _description = description,
            _icon = icon,
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
        string? icon,
        int modifiedBy)
    {
        _name = name ?? _name;
        _description = description ?? _description;
        _icon = icon ?? _icon;
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