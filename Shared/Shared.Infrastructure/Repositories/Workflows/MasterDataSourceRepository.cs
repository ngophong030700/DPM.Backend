using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Persistence;
using Workflow.Domain.MasterDataSources;
using Workflow.Domain.Repositories;

namespace Shared.Infrastructure.Repositories.Workflows
{
    public class MasterDataSourceRepository : IMasterDataSourceRepository
    {
        private readonly ApplicationDbContext _context;

        public MasterDataSourceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MasterDataSource?> GetByIdAsync(int id)
        {
            return await _context.Set<MasterDataSource>()
                .Include(x => x.Columns.Where(c => !c.IsDeleted))
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<MasterDataSource?> GetByCodeAsync(string code)
        {
            return await _context.Set<MasterDataSource>()
                .Include(x => x.Columns.Where(c => !c.IsDeleted))
                .FirstOrDefaultAsync(x => x.Code == code && !x.IsDeleted);
        }

        public async Task<MasterDataSource> CreateAsync(MasterDataSource masterDataSource)
        {
            await _context.Set<MasterDataSource>().AddAsync(masterDataSource);
            await _context.SaveChangesAsync();
            return masterDataSource;
        }

        public async Task<MasterDataSource> UpdateAsync(MasterDataSource masterDataSource)
        {
            _context.Set<MasterDataSource>().Update(masterDataSource);
            await _context.SaveChangesAsync();
            return masterDataSource;
        }

        public async Task<bool> SoftDeleteAsync(int id, int modifiedBy)
        {
            var entity = await _context.Set<MasterDataSource>().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null) return false;

            entity.SoftDelete(modifiedBy);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsCodeExistsAsync(string code, int? excludeId = null)
        {
            return await _context.Set<MasterDataSource>()
                .AnyAsync(x => x.Code == code && x.Id != excludeId && !x.IsDeleted);
        }

        public async Task<MasterDataValue?> GetValueByIdAsync(int valueId)
        {
            return await _context.Set<MasterDataValue>()
                .Include(v => v.Cells)
                .FirstOrDefaultAsync(v => v.Id == valueId && !v.IsDeleted);
        }

        public async Task AddValueAsync(MasterDataValue value)
        {
            await _context.Set<MasterDataValue>().AddAsync(value);
        }

        public void UpdateValue(MasterDataValue value)
        {
            _context.Set<MasterDataValue>().Update(value);
        }

        public async Task<bool> SoftDeleteValueAsync(int valueId, int modifiedBy)
        {
            var entity = await _context.Set<MasterDataValue>().FirstOrDefaultAsync(v => v.Id == valueId && !v.IsDeleted);
            if (entity == null) return false;

            entity.SoftDelete(modifiedBy);
            return true;
        }

        public async Task<MasterDataColumn?> GetColumnByIdAsync(int columnId)
        {
            return await _context.Set<MasterDataColumn>()
                .FirstOrDefaultAsync(c => c.Id == columnId && !c.IsDeleted);
        }

        public async Task AddColumnAsync(MasterDataColumn column)
        {
            await _context.Set<MasterDataColumn>().AddAsync(column);
        }

        public void UpdateColumn(MasterDataColumn column)
        {
            _context.Set<MasterDataColumn>().Update(column);
        }

        public async Task<bool> SoftDeleteColumnAsync(int columnId, int modifiedBy)
        {
            var entity = await _context.Set<MasterDataColumn>().FirstOrDefaultAsync(c => c.Id == columnId && !c.IsDeleted);
            if (entity == null) return false;

            entity.SoftDelete(modifiedBy);
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}