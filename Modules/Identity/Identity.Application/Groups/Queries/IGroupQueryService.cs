using Shared.Application.DTOs.Identity;

namespace Identity.Application.Groups.Queries
{
    public interface IGroupQueryService
    {
        Task<IEnumerable<ViewListGroupDto>> GetListAsync(string? keyword);
        Task<ViewDetailGroupDto?> GetByIdAsync(int id);
    }
}
