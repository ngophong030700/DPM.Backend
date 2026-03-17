using Microsoft.EntityFrameworkCore;
using Shared.Domain.Exceptions;
using Shared.Infrastructure.Persistence;
using Identity.Domain.Users;
using Identity.Domain.Repositories;

namespace Shared.Infrastructure.Repositories.Identities
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(x => x.Department)
                .Include(x => x.Position)
                .Include(x => x.UserGroups)
                    .ThenInclude(ug => ug.Group)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(x => x.UserGroups)
                .FirstOrDefaultAsync(x =>
                    x.Username == username && !x.IsDeleted);
        }

        public async Task<User?> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return await GetUserByIdAsync(user.Id);
        }

        public async Task<User?> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return await GetUserByIdAsync(user.Id);
        }

        public async Task<bool> SoftDeleteUserAsync(
            int id,
            int deletedByUserId)
        {
            var entity = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (entity == null)
                throw new NotFoundException(
                    "Người dùng không tồn tại hoặc đã bị xóa.");

            entity.SoftDelete();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetUsersAsync(
            string? keyword = null,
            int? departmentId = null,
            int? positionId = null,
            bool? isActive = null)
        {
            var query = _context.Users
                .Include(x => x.Department)
                .Include(x => x.Position)
                .AsQueryable();

            query = query.Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x =>
                    x.Username.Contains(keyword) ||
                    x.FullName.Contains(keyword) ||
                    x.Email.Contains(keyword));
            }

            if (departmentId.HasValue)
            {
                query = query.Where(x => x.DepartmentId == departmentId);
            }

            if (positionId.HasValue)
            {
                query = query.Where(x => x.PositionId == positionId);
            }

            if (isActive.HasValue)
            {
                query = query.Where(x => x.IsActive == isActive);
            }

            return await query
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}