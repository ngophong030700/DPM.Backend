using Shared.Domain;

namespace Workflow.Domain.WorkflowDefinitions;

public enum WorkflowVersionStatus
{
    Draft = 0,
    Published = 1,
    Archived = 2
}

public class WorkflowVersion : Entity
{
    private int _id;
    private int _workflowId;
    private string _versionName;
    private bool _isActive;
    private WorkflowVersionStatus _status;
    private string? _notes;
    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;
    private bool _isDeleted;

    public int Id => _id;
    public int WorkflowId => _workflowId;
    public string VersionName => _versionName;
    public bool IsActive => _isActive;
    public WorkflowVersionStatus Status => _status;
    public string? Notes => _notes;
    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;
    public bool IsDeleted => _isDeleted;

    private WorkflowVersion() { }

    internal static WorkflowVersion Create(
        int workflowId,
        string versionName,
        bool isActive,
        string? notes,
        int createdBy)
    {
        return new WorkflowVersion
        {
            _workflowId = workflowId,
            _versionName = versionName ?? throw new ArgumentNullException(nameof(versionName)),
            _isActive = isActive,
            _status = WorkflowVersionStatus.Draft,
            _notes = notes,
            _createdAt = DateTime.UtcNow,
            _modifiedAt = DateTime.UtcNow,
            _createdBy = createdBy,
            _modifiedBy = createdBy,
            _isDeleted = false
        };
    }

    internal void Activate(int modifiedBy)
    {
        _isActive = true;
        _status = WorkflowVersionStatus.Published;
        _modifiedBy = modifiedBy;
        _modifiedAt = DateTime.UtcNow;
    }

    internal void Deactivate(int modifiedBy)
    {
        _isActive = false;
        _modifiedBy = modifiedBy;
        _modifiedAt = DateTime.UtcNow;
    }

    public void Archive(int modifiedBy)
    {
        if (_status == WorkflowVersionStatus.Archived) return;
        _isActive = false;
        _status = WorkflowVersionStatus.Archived;
        _modifiedBy = modifiedBy;
        _modifiedAt = DateTime.UtcNow;
    }

    public void UpdateNotes(string? notes, int modifiedBy)
    {
        _notes = notes;
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
