using Shared.Domain;

namespace Workflow.Domain.WorkflowDefinitions;

public class WorkflowDefinition : Entity, IAggregateRoot
{
    private int _id;
    private string _name;
    private string _code;
    private int? _categoryId;
    private string? _icon;
    private string? _description;
    private string? _permissions;
    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;
    private bool _isDeleted;

    private readonly List<WorkflowVersion> _versions = new();

    public int Id => _id;
    public string Name => _name;
    public string Code => _code;
    public int? CategoryId => _categoryId;
    public string? Icon => _icon;
    public string? Description => _description;
    public string? Permissions => _permissions;
    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;
    public bool IsDeleted => _isDeleted;

    public IReadOnlyCollection<WorkflowVersion> Versions => _versions.AsReadOnly();

    private WorkflowDefinition() { }

    public static WorkflowDefinition Create(
        string name,
        string code,
        int? categoryId,
        string? icon,
        string? description,
        string? permissions,
        int createdBy)
    {
        return new WorkflowDefinition
        {
            _name = name ?? throw new ArgumentNullException(nameof(name)),
            _code = code ?? throw new ArgumentNullException(nameof(code)),
            _categoryId = categoryId,
            _icon = icon,
            _description = description,
            _permissions = permissions,
            _createdAt = DateTime.UtcNow,
            _modifiedAt = DateTime.UtcNow,
            _createdBy = createdBy,
            _modifiedBy = createdBy,
            _isDeleted = false
        };
    }

    public WorkflowVersion AddVersion(string versionName, string? notes, int createdBy)
    {
        var hasActive = _versions.Any(v => v.IsActive);

        var version = WorkflowVersion.Create(
            workflowId: _id,
            versionName: versionName,
            isActive: !hasActive,
            notes: notes,
            createdBy: createdBy);

        _versions.Add(version);
        return version;
    }

    public void ActivateVersion(int versionId, int modifiedBy)
    {
        var target = _versions.FirstOrDefault(v => v.Id == versionId)
            ?? throw new InvalidOperationException($"Version {versionId} not found.");

        foreach (var v in _versions.Where(v => v.IsActive))
            v.Deactivate(modifiedBy);

        target.Activate(modifiedBy);
    }

    public void Update(
        string? name,
        string? code,
        int? categoryId,
        string? icon,
        string? description,
        string? permissions,
        int modifiedBy)
    {
        _name = name ?? _name;
        _code = code ?? _code;
        _categoryId = categoryId ?? _categoryId;
        _icon = icon ?? _icon;
        _description = description ?? _description;
        _permissions = permissions ?? _permissions;
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
