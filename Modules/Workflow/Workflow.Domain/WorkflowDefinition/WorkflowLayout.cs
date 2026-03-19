using Shared.Domain;

namespace Workflow.Domain.WorkflowLayouts;

public class WorkflowLayout : Entity, IAggregateRoot
{
    private int _id;
    private int _versionId;

    /// <summary>
    /// Serialize danh sách LayoutRow thành JSON để lưu DB.
    /// Deserialization do Infrastructure layer xử lý.
    /// </summary>
    private string _rowsJson;

    private string? _attachmentSettingsJson;

    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;

    public int Id => _id;
    public int VersionId => _versionId;
    public string RowsJson => _rowsJson;
    public string? AttachmentSettingsJson => _attachmentSettingsJson;
    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;

    private WorkflowLayout() { }

    public static WorkflowLayout Create(
        int versionId,
        string rowsJson,
        string? attachmentSettingsJson,
        int createdBy)
    {
        return new WorkflowLayout
        {
            _versionId = versionId,
            _rowsJson = rowsJson ?? throw new ArgumentNullException(nameof(rowsJson)),
            _attachmentSettingsJson = attachmentSettingsJson,
            _createdAt = DateTime.UtcNow,
            _modifiedAt = DateTime.UtcNow,
            _createdBy = createdBy,
            _modifiedBy = createdBy
        };
    }

    public void UpdateLayout(string rowsJson, string? attachmentSettingsJson, int modifiedBy)
    {
        _rowsJson = rowsJson ?? throw new ArgumentNullException(nameof(rowsJson));
        _attachmentSettingsJson = attachmentSettingsJson ?? _attachmentSettingsJson;
        _modifiedBy = modifiedBy;
        _modifiedAt = DateTime.UtcNow;
    }
}
