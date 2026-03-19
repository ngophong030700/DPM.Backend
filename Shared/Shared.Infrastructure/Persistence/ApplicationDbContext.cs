using Identity.Domain.Departments;
using Identity.Domain.Groups;
using Identity.Domain.Positions;
using Identity.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Workflow.Domain.WorkflowCategories;
using Workflow.Domain.MasterDataSources;

namespace Shared.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        #region Workflow
        public DbSet<WorkflowCategory> WorkflowCategories { get; set; }
        public DbSet<MasterDataSource> MasterDataSources { get; set; }
        public DbSet<MasterDataColumn> MasterDataColumns { get; set; }
        public DbSet<MasterDataValue> MasterDataValues { get; set; }
        public DbSet<MasterDataCell> MasterDataCells { get; set; }
        #endregion

        #region Identity
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        #endregion

        #region System

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all configurations from the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
