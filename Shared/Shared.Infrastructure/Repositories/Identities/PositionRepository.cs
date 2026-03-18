using Identity.Domain.Positions;
using Identity.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Persistence;

namespace Shared.Infrastructure.Repositories.Identities
{
    public class PositionRepository : IPositionRepository
    {
        private readonly ApplicationDbContext _context;

        public PositionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Position?> GetByIdAsync(int id)
        {
            return await _context.Positions
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<IEnumerable<Position>> GetListAsync(string? keyword = null)
        {
            var query = _context.Positions
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

        public async Task<Position> CreateAsync(Position position)
        {
            _context.Positions.Add(position);
            await _context.SaveChangesAsync();
            return position;
        }

        public async Task<Position> UpdateAsync(Position position)
        {
            _context.Positions.Update(position);
            await _context.SaveChangesAsync();
            return position;
        }

        public async Task<bool> SoftDeleteAsync(int id, int modifiedBy)
        {
            var entity = await _context.Positions
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (entity == null) return false;

            entity.SoftDelete(modifiedBy);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
        {
            return await _context.Positions
                .AnyAsync(x => x.Name == name && x.Id != excludeId && !x.IsDeleted);
        }
    }
}
