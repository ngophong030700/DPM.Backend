using Shared.Domain;

namespace Workflow.Domain.WorkflowDefinitions;

/// <summary>
/// Value object cho cấu hình nguồn dữ liệu của field kiểu Select / MultiSelect / User / Group.
/// Được serialize thành JSON để lưu vào cột DataSourceConfig.
/// </summary>
public class DataSourceConfig : ValueObject
{
    /// <summary>
    /// ID danh mục khi DataSourceType = Master.
    /// </summary>
    public int? CategoryId { get; private init; }

    /// <summary>
    /// Tên cột hiển thị khi lấy dữ liệu từ Master.
    /// </summary>
    public string? DisplayColumn { get; private init; }

    /// <summary>
    /// JSON filter để lọc dữ liệu từ Master (VD: {"status":"active"}).
    /// </summary>
    public string? FilterJson { get; private init; }

    /// <summary>
    /// Danh sách val:label khi DataSourceType = Direct (multiline string).
    /// </summary>
    public string? DirectValues { get; private init; }

    private DataSourceConfig() { }

    public static DataSourceConfig ForMaster(int categoryId, string? displayColumn, string? filterJson)
    {
        return new DataSourceConfig
        {
            CategoryId = categoryId,
            DisplayColumn = displayColumn,
            FilterJson = filterJson
        };
    }

    public static DataSourceConfig ForDirect(string directValues)
    {
        if (string.IsNullOrWhiteSpace(directValues))
            throw new ArgumentNullException(nameof(directValues));

        return new DataSourceConfig
        {
            DirectValues = directValues
        };
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CategoryId;
        yield return DisplayColumn;
        yield return FilterJson;
        yield return DirectValues;
    }
}
