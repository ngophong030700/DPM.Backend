using Shared.Application.BaseClass;
using Shared.Application.DTOs.Identity;

namespace Identity.Application.Users.Queries
{
    public interface IUserQueryService
    {
        Task<bool> IsUsernameExistsAsync(
            string username,
            int excludeId = 0);

        Task<bool> IsEmailExistsAsync(
            string email,
            int excludeId = 0);

        Task<ViewDetailUserDto?> GetUserByIdAsync(int id);

        Task<PagingResponse<ViewListUserDto>> GetListUserAsync(
            PagingRequest request);

        Task<UserFormMetadataDto> GetUserFormMetadataAsync();
    }
}