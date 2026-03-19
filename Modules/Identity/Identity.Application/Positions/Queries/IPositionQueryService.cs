using Shared.Application.BaseClass;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Positions.Queries
{
    public interface IPositionQueryService
    {
        Task<PagingResponse<ViewListPositionDto>> GetListAsync(PagingRequest request);
        Task<ViewDetailPositionDto?> GetByIdAsync(int id);
    }
}
