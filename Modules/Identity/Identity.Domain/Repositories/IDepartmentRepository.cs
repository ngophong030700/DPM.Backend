using Identity.Domain.Departments;

namespace Identity.Domain.Repositories
{
    public interface IDepartmentRepository
    {
        Task<Department?> GetByIdAsync(int id);
        Task<IEnumerable<Department>> GetListAsync(string? keyword = null);
        Task<Department> CreateAsync(Department department);
        Task<Department> UpdateAsync(Department department);
        Task<bool> SoftDeleteAsync(int id, int modifiedBy);
        Task<bool> IsNameExistsAsync(string name, int? excludeId = null);
        Task<int> GetNextIndexAsync(int? parentId);
    }
}
