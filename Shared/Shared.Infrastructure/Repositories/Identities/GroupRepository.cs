using Identity.Domain.Groups;
using Identity.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Persistence;

namespace Shared.Infrastructure.Repositories.Identities
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Group?> GetByIdAsync(int id)
        {
            return await _context.Groups
                .Include(x => x.UserGroups)
                    .ThenInclude(ug => ug.User)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<IEnumerable<Group>> GetListAsync(string? keyword = null)
        {
            var query = _context.Groups
                .Include(x => x.UserGroups)
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }

            return await query
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Group> CreateAsync(Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<Group> UpdateAsync(Group group)
        {
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<bool> SoftDeleteAsync(int id, int modifiedBy)
        {
            var entity = await _context.Groups
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (entity == null) return false;

            entity.SoftDelete(modifiedBy);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
        {
            return await _context.Groups
                .AnyAsync(x => x.Name == name && x.Id != excludeId && !x.IsDeleted);
        }

        public async Task AddUserToGroupAsync(int userId, int groupId, int createdBy)
        {
            var userGroup = UserGroup.Create(userId, groupId, createdBy);
            _context.UserGroups.Add(userGroup);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserFromGroupAsync(int userId, int groupId)
        {
            var entity = await _context.UserGroups
                .FirstOrDefaultAsync(x => x.UserId == userId && x.GroupId == groupId);

            if (entity != null)
            {
                _context.UserGroups.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearUsersInGroupAsync(int groupId)
        {
            var entities = await _context.UserGroups
                .Where(x => x.GroupId == groupId)
                .ToListAsync();

            if (entities.Any())
            {
                _context.UserGroups.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
        }
    }
}
