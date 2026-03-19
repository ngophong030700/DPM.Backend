using Microsoft.EntityFrameworkCore;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.Persistence;
using Workflow.Application.MasterDataSources.Mappings;
using Workflow.Application.MasterDataSources.Queries;

namespace Shared.Infrastructure.QueryServices.Workflows
{
    public class MasterDataSourceQueryService : IMasterDataSourceQueryService
    {
        private readonly ApplicationDbContext _context;

        public MasterDataSourceQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ViewDetailMasterDataSourceDto?> GetByIdAsync(int id)
        {
            var entity = await _context.MasterDataSources
                .Include(x => x.Columns.Where(c => !c.IsDeleted))
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (entity == null) return null;

            var dto = entity.ToDetailDto()!;

            var userIds = new List<int> { dto.CreatedBy, dto.ModifiedBy }.Distinct();
            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new { u.Id, u.FullName })
                .ToListAsync();

            dto.CreatedByName = users.FirstOrDefault(u => u.Id == dto.CreatedBy)?.FullName;
            dto.ModifiedByName = users.FirstOrDefault(u => u.Id == dto.ModifiedBy)?.FullName;

            return dto;
        }

        public async Task<PagingResponse<ViewListMasterDataSourceDto>> GetListAsync(PagingRequest request)
        {
            int page = request.PageNumber ?? 1;
            int size = request.PageSize ?? 20;

            var query = _context.MasterDataSources
                .Where(x => !x.IsDeleted)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                var kw = request.Keyword.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(kw) || x.Code.ToLower().Contains(kw));
            }

            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                query = request.SortDirection == Shared.Domain.Enum.SortDirectionEnum.Asc
                    ? query.OrderByDynamic(request.SortBy, true)
                    : query.OrderByDynamic(request.SortBy, false);
            }
            else
            {
                query = query.OrderByDescending(x => x.CreatedAt);
            }

            int total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(x => x.ToListDto()!)
                .ToListAsync();

            return new PagingResponse<ViewListMasterDataSourceDto>
            {
                Items = items,
                TotalItems = total,
                PageNumber = page,
                PageSize = size
            };
        }

        public async Task<PagingResponse<ViewMasterDataValueDto>> GetValuesAsync(int sourceId, PagingRequest request)
        {
            int page = request.PageNumber ?? 1;
            int size = request.PageSize ?? 20;

            var columns = await _context.MasterDataColumns
                .Where(c => c.SourceId == sourceId && !c.IsDeleted)
                .OrderBy(c => c.SortOrder)
                .ToListAsync();

            var query = _context.MasterDataValues
                .Include(v => v.Cells)
                .Where(v => v.SourceId == sourceId && !v.IsDeleted)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                var kw = request.Keyword.Trim().ToLower();
                query = query.Where(v => v.DisplayName.ToLower().Contains(kw) || v.ValueCode.ToLower().Contains(kw));
            }

            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                query = request.SortDirection == Shared.Domain.Enum.SortDirectionEnum.Asc
                    ? query.OrderByDynamic(request.SortBy, true)
                    : query.OrderByDynamic(request.SortBy, false);
            }
            else
            {
                query = query.OrderBy(v => v.SortOrder);
            }

            int total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            var dtos = items.Select(v => v.ToValueDto(columns)!).ToList();

            return new PagingResponse<ViewMasterDataValueDto>
            {
                Items = dtos,
                TotalItems = total,
                PageNumber = page,
                PageSize = size
            };
        }

        public async Task<ViewMasterDataValueDto?> GetValueByIdAsync(int valueId)
        {
            var value = await _context.MasterDataValues
                .Include(v => v.Cells)
                .FirstOrDefaultAsync(v => v.Id == valueId && !v.IsDeleted);

            if (value == null) return null;

            var columns = await _context.MasterDataColumns
                .Where(c => c.SourceId == value.SourceId && !c.IsDeleted)
                .ToListAsync();

            return value.ToValueDto(columns);
        }
    }
}