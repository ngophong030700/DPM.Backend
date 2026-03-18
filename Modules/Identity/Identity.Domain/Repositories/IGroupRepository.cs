using Identity.Domain.Groups;

namespace Identity.Domain.Repositories
{
    public interface IGroupRepository
    {
        Task<Group?> GetByIdAsync(int id);
        Task<IEnumerable<Group>> GetListAsync(string? keyword = null);
        Task<Group> CreateAsync(Group group);
        Task<Group> UpdateAsync(Group group);
        Task<bool> SoftDeleteAsync(int id, int modifiedBy);
        Task<bool> IsNameExistsAsync(string name, int? excludeId = null);

        Task AddUserToGroupAsync(int userId, int groupId, int createdBy);
        Task RemoveUserFromGroupAsync(int userId, int groupId);
        Task ClearUsersInGroupAsync(int groupId);
    }
}
