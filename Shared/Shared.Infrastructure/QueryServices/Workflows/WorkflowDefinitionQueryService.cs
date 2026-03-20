using Microsoft.EntityFrameworkCore;
using Shared.Application.BaseClass;
using Shared.Application.DTOs.Workflows;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.Persistence;
using Workflow.Application.WorkflowDefinitions.Mappings;
using Workflow.Application.WorkflowDefinitions.Queries;
using Workflow.Domain.WorkflowDefinitions;

namespace Shared.Infrastructure.QueryServices.Workflows
{
    public class WorkflowDefinitionQueryService : IWorkflowDefinitionQueryService
    {
        private readonly ApplicationDbContext _context;

        public WorkflowDefinitionQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ViewDetailWorkflowDefinitionDto?> GetByIdAsync(int id)
        {
            var entity = await _context.WorkflowDefinitions
                .Include(x => x.Versions)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (entity == null) return null;

            var dto = entity.ToDetailDto()!;

            // Get Category Name
            if (dto.CategoryId.HasValue)
            {
                dto.CategoryName = await _context.WorkflowCategories
                    .Where(c => c.Id == dto.CategoryId.Value)
                    .Select(c => c.Name)
                    .FirstOrDefaultAsync();
            }

            // Get User Names
            var userIds = new List<int> { dto.CreatedById, dto.ModifiedById }.Distinct();
            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new { u.Id, u.FullName })
                .ToListAsync();

            dto.CreatedBy = users.FirstOrDefault(u => u.Id == dto.CreatedById)?.FullName;
            dto.ModifiedBy = users.FirstOrDefault(u => u.Id == dto.ModifiedById)?.FullName;

            return dto;
        }

        public async Task<PagingResponse<ViewListWorkflowDefinitionDto>> GetListAsync(PagingRequest request)
        {
            int page = request.PageNumber ?? 1;
            int size = request.PageSize ?? 20;

            var query = _context.WorkflowDefinitions
                .Include(x => x.Versions)
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

            var entities = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            var items = entities.Select(x => x.ToListDto()!).ToList();

            // Populate Category and User names
            var categoryIds = items.Where(x => x.CategoryId.HasValue).Select(x => x.CategoryId!.Value).Distinct();
            var categories = await _context.WorkflowCategories
                .Where(c => categoryIds.Contains(c.Id))
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();

            var userIds = items.Select(x => x.CreatedById).Distinct();
            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new { u.Id, u.FullName })
                .ToListAsync();

            foreach (var item in items)
            {
                item.CategoryName = categories.FirstOrDefault(c => c.Id == item.CategoryId)?.Name;
                item.CreatedBy = users.FirstOrDefault(u => u.Id == item.CreatedById)?.FullName;
            }

            return new PagingResponse<ViewListWorkflowDefinitionDto>
            {
                Items = items,
                TotalItems = total,
                PageNumber = page,
                PageSize = size
            };
        }

        public async Task<ViewWorkflowVersionDto?> GetVersionByIdAsync(int versionId)
        {
            var entity = await _context.WorkflowVersions
                .FirstOrDefaultAsync(x => x.Id == versionId);

            return entity?.ToVersionDto();
        }

        public async Task<List<FieldConfigDto>> GetFieldsByVersionIdAsync(int versionId)
        {
            var fields = await _context.WorkflowFields
                .Include(f => f.GridColumns)
                .Where(x => x.VersionId == versionId && !x.IsDeleted)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            return fields.Select(f => f.ToFieldConfigDto()).ToList();
        }

        public async Task<SetupWorkflowLayoutDto?> GetLayoutByVersionIdAsync(int versionId)
        {
            var layout = await _context.WorkflowLayouts
                .FirstOrDefaultAsync(x => x.VersionId == versionId);

            if (layout == null) return null;

            return new SetupWorkflowLayoutDto
            {
                RowsJson = layout.RowsJson,
                AttachmentSettingsJson = layout.AttachmentSettingsJson
            };
        }

        public async Task<List<StepConfigDto>> GetStepsByVersionIdAsync(int versionId)
        {
            var steps = await _context.WorkflowStepDefines
                .Include(s => s.Actions).ThenInclude(a => a.Rules)
                .Include(s => s.Documents)
                .Include(s => s.FieldPermissions)
                .Include(s => s.Hooks)
                .Where(x => x.VersionId == versionId && !x.IsDeleted)
                .ToListAsync();

            return steps.Select(s => s.ToStepConfigDto()).ToList();
        }

        public async Task<List<ViewWorkflowReportDto>> GetReportsByVersionIdAsync(int versionId)
        {
            var reports = await _context.WorkflowReports
                .Where(x => x.VersionId == versionId && !x.IsDeleted)
                .ToListAsync();

            return reports.Select(r => r.ToReportDto()).ToList();
        }

        public async Task<WorkflowDefinitionMetadataDto> GetDefinitionMetadataAsync()
        {
            var metadata = new WorkflowDefinitionMetadataDto();

            metadata.Categories = await _context.WorkflowCategories
                .Where(x => !x.IsDeleted)
                .Select(x => new MetadataItemDto { Id = x.Id.ToString(), Name = x.Name })
                .ToListAsync();

            metadata.Users = await _context.Users
                .Where(x => !x.IsDeleted)
                .Select(x => new MetadataItemDto { Id = x.Id.ToString(), Name = x.FullName })
                .ToListAsync();

            metadata.Groups = await _context.Groups
                .Where(x => !x.IsDeleted)
                .Select(x => new MetadataItemDto { Id = x.Id.ToString(), Name = x.Name })
                .ToListAsync();

            metadata.Icons = new List<string> { "setting", "user", "file", "workflow" }; // Placeholder icons

            return metadata;
        }

        public async Task<WorkflowFieldMetadataDto> GetFieldMetadataAsync()
        {
            var metadata = new WorkflowFieldMetadataDto();

            metadata.MasterDataSources = await _context.MasterDataSources
                .Include(x => x.Columns)
                .Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new MasterDataSourceMetadataDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Columns = x.Columns.Where(c => !c.IsDeleted).Select(c => c.ColumnKey).ToList()
                })
                .ToListAsync();

            metadata.FieldDataTypes = Enum.GetValues<FieldDataType>()
                .Select(e => new MetadataItemDto { Id = ((int)e).ToString(), Name = e.ToString() })
                .ToList();

            return metadata;
        }

        public async Task<WorkflowStepMetadataDto> GetStepMetadataAsync(int versionId)
        {
            var metadata = new WorkflowStepMetadataDto();

            // Placeholder for real logic
            metadata.NotificationTemplates = new List<MetadataItemDto> 
            { 
                new MetadataItemDto { Id = "1", Name = "Mẫu thông báo duyệt" },
                new MetadataItemDto { Id = "2", Name = "Mẫu thông báo từ chối" }
            };

            metadata.Users = await _context.Users
                .Where(x => !x.IsDeleted)
                .Select(x => new MetadataItemDto { Id = x.Id.ToString(), Name = x.FullName })
                .ToListAsync();

            metadata.Groups = await _context.Groups
                .Where(x => !x.IsDeleted)
                .Select(x => new MetadataItemDto { Id = x.Id.ToString(), Name = x.Name })
                .ToListAsync();

            metadata.DocumentTypes = new List<MetadataItemDto>
            {
                new MetadataItemDto { Id = "invoice", Name = "Hóa đơn" },
                new MetadataItemDto { Id = "contract", Name = "Hợp đồng" }
            };

            metadata.UserFields = await _context.WorkflowFields
                .Where(x => x.VersionId == versionId && !x.IsDeleted && (x.DataType == FieldDataType.User))
                .Select(x => new MetadataItemDto { Id = x.Name, Name = x.Label })
                .ToListAsync();

            return metadata;
        }
    }
}