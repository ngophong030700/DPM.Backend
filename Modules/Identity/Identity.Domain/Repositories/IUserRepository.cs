using Identity.Domain.Users;

namespace Identity.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);

        Task<User?> GetUserByUsernameAsync(string username);

        Task<User?> CreateUserAsync(User user);

        Task<User?> UpdateUserAsync(User user);

        Task<bool> SoftDeleteUserAsync(int id, int deletedByUserId);

        Task<IEnumerable<User>> GetUsersAsync(
            string? keyword = null,
            int? departmentId = null,
            int? positionId = null,
            bool? isActive = null);

        Task AddUserToGroupAsync(int userId, int groupId, int createdBy);
        Task ClearGroupsForUserAsync(int userId);
        }
        }