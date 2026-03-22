using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Persistence;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowDefinitions;

namespace Shared.Infrastructure.Repositories.Workflows
{
    public class WorkflowDefinitionRepository : IWorkflowDefinitionRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkflowDefinitionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WorkflowDefinition?> GetByIdAsync(int id, bool includeDetails = false)
        {
            var query = _context.WorkflowDefinitions.AsQueryable();

            if (includeDetails)
            {
                query = query.Include(x => x.Versions);
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<WorkflowDefinition?> GetByCodeAsync(string code, bool includeDetails = false)
        {
            var query = _context.WorkflowDefinitions.AsQueryable();

            if (includeDetails)
            {
                query = query.Include(x => x.Versions);
            }

            return await query.FirstOrDefaultAsync(x => x.Code == code && !x.IsDeleted);
        }

        public async Task<WorkflowDefinition> CreateAsync(WorkflowDefinition workflowDefinition)
        {
            await _context.WorkflowDefinitions.AddAsync(workflowDefinition);
            await _context.SaveChangesAsync();
            return workflowDefinition;
        }

        public async Task<WorkflowDefinition> UpdateAsync(WorkflowDefinition workflowDefinition)
        {
            _context.WorkflowDefinitions.Update(workflowDefinition);
            await _context.SaveChangesAsync();
            return workflowDefinition;
        }

        public async Task<bool> SoftDeleteAsync(int id, int modifiedBy)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            entity.SoftDelete(modifiedBy);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsCodeExistsAsync(string code, int? excludeId = null)
        {
            return await _context.WorkflowDefinitions
                .AnyAsync(x => x.Code == code && x.Id != excludeId && !x.IsDeleted);
        }

        public async Task<WorkflowVersion?> GetVersionByIdAsync(int versionId)
        {
            return await _context.WorkflowVersions
                .FirstOrDefaultAsync(x => x.Id == versionId);
        }

        public async Task<WorkflowVersion> CreateVersionAsync(WorkflowVersion version)
        {
            await _context.WorkflowVersions.AddAsync(version);
            await _context.SaveChangesAsync();
            return version;
        }

        public async Task UpdateVersionAsync(WorkflowVersion version)
        {
            _context.WorkflowVersions.Update(version);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> SoftDeleteVersionAsync(int versionId, int modifiedBy)
        {
            var version = await GetVersionByIdAsync(versionId);
            if (version == null) return false;
            version.SoftDelete(modifiedBy);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<WorkflowField>> GetFieldsByVersionIdAsync(int versionId)
        {
            return await _context.WorkflowFields
                .Include(f => f.GridColumns)
                .Where(x => x.VersionId == versionId && !x.IsDeleted)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();
        }

        public async Task<WorkflowField?> GetFieldByIdAsync(int id)
        {
            return await _context.WorkflowFields
                .Include(f => f.GridColumns)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task SaveFieldsAsync(int versionId, List<WorkflowField> fields)
        {
            var existingFields = await _context.WorkflowFields
                .Include(f => f.GridColumns)
                .Where(x => x.VersionId == versionId && !x.IsDeleted)
                .ToListAsync();

            // 1. Delete fields not in the new list
            var newFieldIds = fields.Where(f => f.Id > 0).Select(f => f.Id).ToList();
            var fieldsToDelete = existingFields.Where(f => !newFieldIds.Contains(f.Id)).ToList();
            foreach (var f in fieldsToDelete) f.SoftDelete(0);

            // 2. Update or Create
            foreach (var field in fields)
            {
                if (field.Id > 0)
                {
                    var existing = existingFields.FirstOrDefault(f => f.Id == field.Id);
                    if (existing != null)
                    {
                        _context.Entry(existing).CurrentValues.SetValues(field);
                        _context.Entry(existing).State = EntityState.Modified;
                    }
                }
                else
                {
                    await _context.WorkflowFields.AddAsync(field);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task SaveFieldAsync(WorkflowField field)
        {
            if (field.Id > 0)
            {
                _context.WorkflowFields.Update(field);
            }
            else
            {
                await _context.WorkflowFields.AddAsync(field);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteFieldAsync(int id, int modifiedBy)
        {
            var field = await GetFieldByIdAsync(id);
            if (field == null) return false;
            field.SoftDelete(modifiedBy);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<WorkflowGridColumn?> GetGridColumnByIdAsync(int id)
        {
            return await _context.Set<WorkflowGridColumn>()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task SaveGridColumnAsync(WorkflowGridColumn column)
        {
            if (column.Id > 0)
            {
                _context.Set<WorkflowGridColumn>().Update(column);
            }
            else
            {
                await _context.Set<WorkflowGridColumn>().AddAsync(column);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteGridColumnAsync(int id, int modifiedBy)
        {
            var column = await GetGridColumnByIdAsync(id);
            if (column == null) return false;
            column.SoftDelete(modifiedBy);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<WorkflowLayout?> GetLayoutByVersionIdAsync(int versionId)
        {
            return await _context.WorkflowLayouts
                .FirstOrDefaultAsync(x => x.VersionId == versionId);
        }

        public async Task SaveLayoutAsync(WorkflowLayout layout)
        {
            var existing = await GetLayoutByVersionIdAsync(layout.VersionId);
            if (existing != null)
            {
                _context.WorkflowLayouts.Remove(existing);
            }
            await _context.WorkflowLayouts.AddAsync(layout);
            await _context.SaveChangesAsync();
        }

        public async Task<List<WorkflowStepDefine>> GetStepsByVersionIdAsync(int versionId)
        {
            return await _context.WorkflowStepDefines
                .Include(s => s.Actions).ThenInclude(a => a.Rules)
                .Include(s => s.Documents)
                .Include(s => s.FieldPermissions)
                .Include(s => s.Hooks)
                .Where(x => x.VersionId == versionId && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<WorkflowStepDefine?> GetStepByIdAsync(string id, int versionId)
        {
            return await _context.WorkflowStepDefines
                .Include(s => s.Actions).ThenInclude(a => a.Rules)
                .Include(s => s.Documents)
                .Include(s => s.FieldPermissions)
                .Include(s => s.Hooks)
                .FirstOrDefaultAsync(x => x.Id == id && x.VersionId == versionId && !x.IsDeleted);
        }

        public async Task SaveStepsAsync(int versionId, List<WorkflowStepDefine> steps)
        {
            var existingSteps = await GetStepsByVersionIdAsync(versionId);
            
            var newStepIds = steps.Select(s => s.Id).ToList();
            var stepsToDelete = existingSteps.Where(s => !newStepIds.Contains(s.Id)).ToList();
            _context.WorkflowStepDefines.RemoveRange(stepsToDelete);

            foreach (var step in steps)
            {
                var existing = existingSteps.FirstOrDefault(s => s.Id == step.Id);
                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(step);
                }
                else
                {
                    await _context.WorkflowStepDefines.AddAsync(step);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task SaveStepAsync(WorkflowStepDefine step)
        {
            var existing = await _context.WorkflowStepDefines.FindAsync(step.Id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(step);
            }
            else
            {
                await _context.WorkflowStepDefines.AddAsync(step);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteStepAsync(string id, int versionId, int modifiedBy)
        {
            var step = await GetStepByIdAsync(id, versionId);
            if (step == null) return false;
            step.SoftDelete(modifiedBy);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<WorkflowStepDefineAction?> GetActionByIdAsync(int id)
        {
            return await _context.Set<WorkflowStepDefineAction>()
                .Include(a => a.Rules)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task SaveActionAsync(WorkflowStepDefineAction action)
        {
            if (action.Id > 0)
            {
                _context.Set<WorkflowStepDefineAction>().Update(action);
            }
            else
            {
                await _context.Set<WorkflowStepDefineAction>().AddAsync(action);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteActionAsync(int id, int modifiedBy)
        {
            var action = await GetActionByIdAsync(id);
            if (action == null) return false;
            action.SoftDelete(modifiedBy);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<WorkflowStepDefineDocument?> GetDocumentByIdAsync(int id)
        {
            return await _context.Set<WorkflowStepDefineDocument>()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task SaveDocumentAsync(WorkflowStepDefineDocument document)
        {
            if (document.Id > 0)
            {
                _context.Set<WorkflowStepDefineDocument>().Update(document);
            }
            else
            {
                await _context.Set<WorkflowStepDefineDocument>().AddAsync(document);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteDocumentAsync(int id, int modifiedBy)
        {
            var doc = await GetDocumentByIdAsync(id);
            if (doc == null) return false;
            doc.SoftDelete(modifiedBy);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<WorkflowReport>> GetReportsByVersionIdAsync(int versionId)
        {
            return await _context.WorkflowReports
                .Where(x => x.VersionId == versionId && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<WorkflowReport?> GetReportByIdAsync(int id)
        {
            return await _context.WorkflowReports
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task SaveReportAsync(WorkflowReport report)
        {
            if (report.Id > 0)
            {
                var existing = await _context.WorkflowReports.FindAsync(report.Id);
                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(report);
                }
            }
            else
            {
                await _context.WorkflowReports.AddAsync(report);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteReportAsync(int reportId, int modifiedBy)
        {
            var report = await _context.WorkflowReports.FindAsync(reportId);
            if (report == null) return false;
            report.SoftDelete(modifiedBy);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
