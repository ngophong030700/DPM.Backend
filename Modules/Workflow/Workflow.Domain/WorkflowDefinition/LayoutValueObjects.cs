using Shared.Domain;

namespace Workflow.Domain.WorkflowLayouts;

/// <summary>
/// Một ô trong layout. colSpan theo grid 12 cột.
/// </summary>
public class LayoutColumn : ValueObject
{
    public int FieldId { get; private init; }
    public int ColSpan { get; private init; }  // 1–12
    public int Offset { get; private init; }   // offset trái (0–11)

    private LayoutColumn() { }

    public static LayoutColumn Create(int fieldId, int colSpan, int offset = 0)
    {
        if (colSpan < 1 || colSpan > 12)
            throw new ArgumentOutOfRangeException(nameof(colSpan), "ColSpan must be between 1 and 12.");
        if (offset < 0 || offset > 11)
            throw new ArgumentOutOfRangeException(nameof(offset));

        return new LayoutColumn { FieldId = fieldId, ColSpan = colSpan, Offset = offset };
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FieldId;
        yield return ColSpan;
        yield return Offset;
    }
}

/// <summary>
/// Một hàng trong layout, chứa nhiều cột.
/// </summary>
public class LayoutRow : ValueObject
{
    public string RowId { get; private init; }
    public IReadOnlyList<LayoutColumn> Columns { get; private init; }

    private LayoutRow() { }

    public static LayoutRow Create(string rowId, IEnumerable<LayoutColumn> columns)
    {
        var cols = columns?.ToList() ?? throw new ArgumentNullException(nameof(columns));
        if (!cols.Any()) throw new ArgumentException("A row must have at least one column.");

        return new LayoutRow
        {
            RowId = rowId ?? throw new ArgumentNullException(nameof(rowId)),
            Columns = cols.AsReadOnly()
        };
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return RowId;
        foreach (var col in Columns)
            foreach (var c in col.GetEqualityComponents())
                yield return c;
    }
}

/// <summary>
/// Cấu hình file đính kèm của form.
/// </summary>
public class AttachmentSettings : ValueObject
{
    public bool IsVisible { get; private init; }
    public IReadOnlyList<string> AllowTypes { get; private init; }  // VD: [".pdf", ".docx"]
    public long MaxFileSizeBytes { get; private init; }

    private AttachmentSettings() { }

    public static AttachmentSettings Create(bool isVisible, IEnumerable<string>? allowTypes, long maxFileSizeBytes)
    {
        return new AttachmentSettings
        {
            IsVisible = isVisible,
            AllowTypes = (allowTypes ?? Enumerable.Empty<string>()).ToList().AsReadOnly(),
            MaxFileSizeBytes = maxFileSizeBytes
        };
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return IsVisible;
        yield return MaxFileSizeBytes;
        foreach (var t in AllowTypes) yield return t;
    }
}
