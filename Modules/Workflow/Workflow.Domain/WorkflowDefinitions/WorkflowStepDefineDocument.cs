using Shared.Domain;

namespace Workflow.Domain.WorkflowDefinitions;

public class WorkflowStepDefineDocument : Entity
{
    private int _id;
    private string _stepId;
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
    public string StepId => _stepId;
    public string DocTypeName => _docTypeName;
    public bool IsRequired => _isRequired;
    public bool CheckDigitalSignature => _checkDigitalSignature;
    public int SortOrder => _sortOrder;
    public DateTime CreatedAt => _createdAt;
    public int CreatedBy => _createdBy;
    public DateTime ModifiedAt => _modifiedAt;
    public int ModifiedBy => _modifiedBy;
    public bool IsDeleted => _isDeleted;

    private WorkflowStepDefineDocument() { }

    public static WorkflowStepDefineDocument Create(
        string stepId,
        string docTypeName,
        bool isRequired,
        bool checkDigitalSignature,
        int createdBy,
        int sortOrder = 0)
    {
        return new WorkflowStepDefineDocument
        {
            _stepId = stepId ?? throw new ArgumentNullException(nameof(stepId)),
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
