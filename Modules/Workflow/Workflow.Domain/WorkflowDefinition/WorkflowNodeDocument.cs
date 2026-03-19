using Shared.Domain;

namespace Workflow.Domain.WorkflowNodes;

public class WorkflowNodeDocument : Entity
{
    private int _id;
    private string _nodeId;
    private string _docTypeName;
    private bool _isRequired;
    private bool _checkDigitalSignature;
    private int _sortOrder;
    private DateTime _createdAt;
    private int _createdBy;
    private DateTime _modifiedAt;
    private int _modifiedBy;
    private bool _isDeleted;

    public int Id => _id;
    public string NodeId => _nodeId;
    public string DocTypeName => _docTypeName;
    public bool IsRequired => _isRequired;
    public bool CheckDigitalSignature => _checkDigitalSignature;
    public int SortOrder => _sortOrder;
    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;
    public bool IsDeleted => _isDeleted;

    private WorkflowNodeDocument() { }

    internal static WorkflowNodeDocument Create(
        string nodeId,
        string docTypeName,
        bool isRequired,
        bool checkDigitalSignature,
        int createdBy,
        int sortOrder = 0)
    {
        return new WorkflowNodeDocument
        {
            _nodeId = nodeId ?? throw new ArgumentNullException(nameof(nodeId)),
            _docTypeName = docTypeName ?? throw new ArgumentNullException(nameof(docTypeName)),
            _isRequired = isRequired,
            _checkDigitalSignature = checkDigitalSignature,
            _sortOrder = sortOrder,
            _createdAt = DateTime.UtcNow,
            _modifiedAt = DateTime.UtcNow,
            _createdBy = createdBy,
            _modifiedBy = createdBy,
            _isDeleted = false
        };
    }

    public void Update(
        string? docTypeName,
        bool? isRequired,
        bool? checkDigitalSignature,
        int? sortOrder,
        int modifiedBy)
    {
        _docTypeName = docTypeName ?? _docTypeName;
        _isRequired = isRequired ?? _isRequired;
        _checkDigitalSignature = checkDigitalSignature ?? _checkDigitalSignature;
        _sortOrder = sortOrder ?? _sortOrder;
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
