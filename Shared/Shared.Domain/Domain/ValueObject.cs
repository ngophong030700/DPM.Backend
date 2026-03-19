namespace Shared.Domain;

/// <summary>
/// Base class cho Value Object theo DDD.
/// Equality dựa trên giá trị các thuộc tính, không phải reference.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Trả về các thành phần dùng để so sánh equality.
    /// Implement bằng cách yield return từng property.
    /// </summary>
    public abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        return ((ValueObject)obj)
            .GetEqualityComponents()
            .SequenceEqual(GetEqualityComponents());
    }

    public override int GetHashCode()
        => GetEqualityComponents()
            .Aggregate(0, (hash, component)
                => HashCode.Combine(hash, component?.GetHashCode() ?? 0));

    public static bool operator ==(ValueObject? left, ValueObject? right)
        => left?.Equals(right) ?? right is null;

    public static bool operator !=(ValueObject? left, ValueObject? right)
        => !(left == right);
}
