using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Persistence;
using Workflow.Domain.Repositories;
using Workflow.Domain.WorkflowCategories;

namespace Shared.Infrastructure.Repositories.Workflows
{
    public class WorkflowCategoryRepository : IWorkflowCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkflowCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WorkflowCategory?> GetByIdAsync(int id)
        {
            return await _context.Set<WorkflowCategory>()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<WorkflowCategory> CreateAsync(WorkflowCategory workflowCategory)
        {
            await _context.Set<WorkflowCategory>().AddAsync(workflowCategory);
            await _context.SaveChangesAsync();
            return workflowCategory;
        }

        public async Task<WorkflowCategory> UpdateAsync(WorkflowCategory workflowCategory)
        {
            _context.Set<WorkflowCategory>().Update(workflowCategory);
            await _context.SaveChangesAsync();
            return workflowCategory;
        }

        public async Task<bool> SoftDeleteAsync(int id, int modifiedBy)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            entity.SoftDelete(modifiedBy);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
        {
            return await _context.Set<WorkflowCategory>()
                .AnyAsync(x => x.Name == name && x.Id != excludeId && !x.IsDeleted);
        }
    }
}