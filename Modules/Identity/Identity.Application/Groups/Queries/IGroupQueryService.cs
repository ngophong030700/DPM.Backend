using Shared.Application.BaseClass;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Groups.Queries
{
    public interface IGroupQueryService
    {
        Task<PagingResponse<ViewListGroupDto>> GetListAsync(PagingRequest request);
        Task<ViewDetailGroupDto?> GetByIdAsync(int id);
    }
}
