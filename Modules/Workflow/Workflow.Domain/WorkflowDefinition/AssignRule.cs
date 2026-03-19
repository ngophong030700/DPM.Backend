namespace Workflow.Domain.WorkflowNodes;

public enum AssignRule
{
    Creator = 0,        // Người tạo đơn
    Manager = 1,        // Quản lý trực tiếp của người tạo
    HandPickUser = 2,   // Chỉ định thủ công theo User ID
    HandPickGroup = 3,  // Chỉ định thủ công theo Group ID
    FieldBased = 4      // Lấy người xử lý từ một field trong form
}

public enum SlaUnit
{
    Minutes = 0,
    Hours = 1,
    Days = 2
}
