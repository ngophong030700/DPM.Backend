using Shared.Application.DTOs.Identity;

namespace Identity.Application.Positions.Queries
{
    public interface IPositionQueryService
    {
        Task<IEnumerable<ViewListPositionDto>> GetListAsync(string? keyword);
        Task<ViewDetailPositionDto?> GetByIdAsync(int id);
    }
}
