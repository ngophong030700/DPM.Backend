using Identity.Domain.Positions;

namespace Identity.Domain.Repositories
{
    public interface IPositionRepository
    {
        Task<Position?> GetByIdAsync(int id);
        Task<IEnumerable<Position>> GetListAsync(string? keyword = null);
        Task<Position> CreateAsync(Position position);
        Task<Position> UpdateAsync(Position position);
        Task<bool> SoftDeleteAsync(int id, int modifiedBy);
        Task<bool> IsNameExistsAsync(string name, int? excludeId = null);
    }
}
