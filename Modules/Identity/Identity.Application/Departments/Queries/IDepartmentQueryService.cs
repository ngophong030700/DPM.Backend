using Shared.Application.DTOs.Identity;

namespace Identity.Application.Departments.Queries
{
    public interface IDepartmentQueryService
    {
        Task<IEnumerable<ViewListDepartmentDto>> GetListAsync(string? keyword);
        Task<ViewDetailDepartmentDto?> GetByIdAsync(int id);
    }
}
