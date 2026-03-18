using Identity.Domain.Departments;
using Identity.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Persistence;

namespace Shared.Infrastructure.Repositories.Identities
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments
                .Include(x => x.Parent)
                .Include(x => x.Childrens)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<IEnumerable<Department>> GetListAsync(string? keyword = null)
        {
            var query = _context.Departments
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }

            return await query
                .OrderBy(x => x.Level)
                .ThenBy(x => x.Index)
                .ToListAsync();
        }

        public async Task<Department> CreateAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<Department> UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<bool> SoftDeleteAsync(int id, int modifiedBy)
        {
            var entity = await _context.Departments
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (entity == null) return false;

            entity.SoftDelete(modifiedBy);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
        {
            return await _context.Departments
                .AnyAsync(x => x.Name == name && x.Id != excludeId && !x.IsDeleted);
        }

        public async Task<int> GetNextIndexAsync(int? parentId)
        {
            var maxIndex = await _context.Departments
                .Where(x => x.ParentId == parentId && !x.IsDeleted)
                .Select(x => (int?)x.Index)
                .MaxAsync();

            return (maxIndex ?? 0) + 1;
        }
    }
}
