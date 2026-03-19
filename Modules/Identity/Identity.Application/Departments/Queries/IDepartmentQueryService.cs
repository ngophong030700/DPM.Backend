using Shared.Application.BaseClass;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Departments.Queries
{
    public interface IDepartmentQueryService
    {
        Task<PagingResponse<ViewListDepartmentDto>> GetListAsync(PagingRequest request);
        Task<ViewDetailDepartmentDto?> GetByIdAsync(int id);
    }
}
