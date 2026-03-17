using Identity.Application.Users.Mappings;
using Identity.Application.Users.Queries;
using Microsoft.EntityFrameworkCore;
using Shared.Application.BaseClass;
using Shared.Application.Common.Interfaces;
using Shared.Application.DTOs.Identity;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.Persistence;

namespace Shared.Infrastructure.QueryServices.Identity
{
    public class UserQueryService : IUserQueryService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UserQueryService(
            ApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        #region ================= VALIDATION =================

        public async Task<bool> IsUsernameExistsAsync(
            string username,
            int excludeId = 0)
        {
            return await _context.Users.AnyAsync(x =>
                x.Username == username &&
                x.Id != excludeId &&
                !x.IsDeleted);
        }

        public async Task<bool> IsEmailExistsAsync(
            string email,
            int excludeId = 0)
        {
            return await _context.Users.AnyAsync(x =>
                x.Email == email &&
                x.Id != excludeId &&
                !x.IsDeleted);
        }

        #endregion

        #region ================= DETAIL =================

        public async Task<ViewDetailUserDto?> GetUserByIdAsync(int id)
        {
            var entity = await _context.Users
                .Include(x => x.Department)
                .Include(x => x.Position)
                .Include(x => x.UserGroups)
                    .ThenInclude(ug => ug.Group)
                .Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();

            var dto = entity?.ToDetailDto();
            if (dto == null)
                return null;

            // ===== Fill CreatedBy / ModifiedBy Name =====
            var userIds = new List<int>
            {
                dto.CreatedById,
                dto.ModifiedById
            }.Distinct();

            var users = await _context.Users
                .Where(x => userIds.Contains(x.Id))
                .Select(x => new { x.Id, x.FullName })
                .ToListAsync();

            dto.CreatedBy = users
                .FirstOrDefault(x => x.Id == dto.CreatedById)?.FullName;

            dto.ModifiedBy = users
                .FirstOrDefault(x => x.Id == dto.ModifiedById)?.FullName;

            return dto;
        }

        #endregion

        #region ================= LIST + PAGING =================

        public async Task<PagingResponse<ViewListUserDto>>
            GetListUserAsync(PagingRequest request)
        {
            int page = request.PageNumber ?? 1;
            int size = request.PageSize ?? int.MaxValue;

            var query = _context.Users
                .Include(x => x.Department)
                .Include(x => x.Position)
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            // ===== FILTER =====
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var kw = request.Keyword.Trim().ToLower();
                query = query.Where(x =>
                    x.Username.ToLower().Contains(kw) ||
                    x.FullName.ToLower().Contains(kw) ||
                    x.Email.ToLower().Contains(kw));
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
                .Select(x => x.ToListDto())
                .ToListAsync();

            // ===== Fill CreatedBy Name =====
            var userIds = items
                .Where(x => x.CreatedById.HasValue)
                .Select(x => x.CreatedById!.Value)
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

            return new PagingResponse<ViewListUserDto>
            {
                Items = items,
                TotalItems = total,
                PageNumber = page,
                PageSize = size
            };
        }

        #endregion

        #region ================= FORM METADATA =================

        public async Task<UserFormMetadataDto> GetUserFormMetadataAsync()
        {
            var dto = new UserFormMetadataDto();

            dto.Departments = await _context.Departments
                .Where(x => !x.IsDeleted)
                .Select(x => new BaseDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            dto.Positions = await _context.Positions
                .Where(x => !x.IsDeleted)
                .Select(x => new BaseDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            dto.Groups = await _context.Groups
                .Where(x => !x.IsDeleted)
                .Select(x => new BaseDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return dto;
        }

        #endregion
    }
}