using Identity.Application.Positions.Mappings;
using Identity.Application.Positions.Queries;
using Microsoft.EntityFrameworkCore;
using Shared.Application.DTOs.Identity;
using Shared.Infrastructure.Persistence;

namespace Shared.Infrastructure.QueryServices.Identity
{
    public class PositionQueryService : IPositionQueryService
    {
        private readonly ApplicationDbContext _context;

        public PositionQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ViewListPositionDto>> GetListAsync(string? keyword)
        {
            var query = _context.Positions
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }

            var items = await query
                .OrderBy(x => x.Name)
                .Select(x => x.ToListDto()!)
                .ToListAsync();

            // Fill CreatedBy Name
            var userIds = items.Select(x => x.CreatedBy).Distinct().ToList();
            var users = await _context.Users
                .Where(x => userIds.Contains(x.Id))
                .Select(x => new { x.Id, x.FullName })
                .ToListAsync();

            foreach (var item in items)
            {
                item.CreatedByName = users.FirstOrDefault(x => x.Id == item.CreatedBy)?.FullName;
            }

            return items;
        }

        public async Task<ViewDetailPositionDto?> GetByIdAsync(int id)
        {
            var entity = await _context.Positions
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (entity == null) return null;

            var dto = entity.ToDetailDto()!;

            // Fill CreatedBy/ModifiedBy Name
            var userIds = new List<int> { dto.CreatedBy, dto.ModifiedBy }.Distinct().ToList();
            var users = await _context.Users
                .Where(x => userIds.Contains(x.Id))
                .Select(x => new { x.Id, x.FullName })
                .ToListAsync();

            dto.CreatedByName = users.FirstOrDefault(x => x.Id == dto.CreatedBy)?.FullName;
            dto.ModifiedByName = users.FirstOrDefault(x => x.Id == dto.ModifiedBy)?.FullName;

            return dto;
        }
    }
}
