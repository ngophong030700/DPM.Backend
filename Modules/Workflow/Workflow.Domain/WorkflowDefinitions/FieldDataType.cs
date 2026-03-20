namespace Workflow.Domain.WorkflowDefinitions;

public enum FieldDataType
{
    Text = 0,
    Number = 1,
    Date = 2,
    Select = 3,
    MultiSelect = 4,
    User = 5,
    Group = 6,
    Grid = 7,
    Formula = 8
}

public enum DataSourceType
{
    Direct = 0,   // Nhập thủ công dạng "val:label" multiline
    Master = 1    // Lấy từ danh mục MasterData
}
