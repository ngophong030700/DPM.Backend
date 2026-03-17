using Identity.Domain.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations
{
    public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.ToTable("user_group");

            builder.HasKey(x => new { x.UserId, x.GroupId });

            builder.Property(x => x.UserId)
                .HasColumnName("user_id");

            builder.Property(x => x.GroupId)
                .HasColumnName("group_id");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.CreatedBy)
                .HasColumnName("created_by");

            builder.Property(x => x.ModifiedAt)
                .HasColumnName("modified_at")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.ModifiedBy)
                .HasColumnName("modified_by");

            // 🔗 User
            builder.HasOne(x => x.User)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔗 Group
            builder.HasOne(x => x.Group)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}