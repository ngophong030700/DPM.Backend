using Microsoft.EntityFrameworkCore;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.Persistence;
using Workflow.Application.WorkflowCategories.Mappings;
using Workflow.Application.WorkflowCategories.Queries;

namespace Shared.Infrastructure.QueryServices.Workflows
{
    public class WorkflowCategoryQueryService : IWorkflowCategoryQueryService
    {
        private readonly ApplicationDbContext _context;

        public WorkflowCategoryQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region ================= DETAIL =================

        public async Task<ViewDetailWorkflowCategoryDto?> GetByIdAsync(int id)
        {
            var entity = await _context.WorkflowCategories
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (entity == null) return null;

            var dto = entity.ToDetailDto()!;

            // ===== Fill CreatedBy / ModifiedBy Name =====
            var userIds = new List<int> { dto.CreatedById, dto.ModifiedById }.Distinct();

            var users = await _context.Users
                .Where(x => userIds.Contains(x.Id))
                .Select(x => new { x.Id, x.FullName })
                .ToListAsync();

            dto.CreatedBy = users.FirstOrDefault(x => x.Id == dto.CreatedById)?.FullName;
            dto.ModifiedBy = users.FirstOrDefault(x => x.Id == dto.ModifiedById)?.FullName;

            return dto;
        }

        #endregion

        #region ================= LIST + PAGING =================

        public async Task<PagingResponse<ViewListWorkflowCategoryDto>> GetListAsync(PagingRequest request)
        {
            int page = request.PageNumber ?? 1;
            int size = request.PageSize ?? int.MaxValue;

            var query = _context.WorkflowCategories
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            // ===== FILTER =====
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var kw = request.Keyword.Trim().ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(kw) ||
                    (x.Description != null && x.Description.ToLower().Contains(kw)));
            }

            // ===== SORT =====
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

            // ===== Fill CreatedBy Name =====
            var userIds = items
                .Select(x => x.CreatedById)
                .Distinct()
                .ToList();

            var users = await _context.Users
                .Where(x => userIds.Contains(x.Id))
                .Select(x => new { x.Id, x.FullName })
                .ToListAsync();

            foreach (var item in items)
            {
                if (item?.CreatedById != null)
                {
                    item.CreatedBy = users
                        .FirstOrDefault(x => x.Id == item.CreatedById)?.FullName;
                }
            }

            return new PagingResponse<ViewListWorkflowCategoryDto>
            {
                Items = items,
                TotalItems = total,
                PageNumber = page,
                PageSize = size
            };
        }

        #endregion
    }
}