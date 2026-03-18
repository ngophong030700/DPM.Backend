using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Identity.Domain.Users;

namespace Shared.Infrastructure.Persistence.Configurations.Identities
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.HasKey(u => u.Id);

            builder.Property(e => e.Username).HasColumnName("username");
            builder.Property(e => e.PasswordHash).HasColumnName("password_hash");
            builder.Property(e => e.FullName).HasColumnName("full_name");
            builder.Property(e => e.Email).HasColumnName("email");
            builder.Property(e => e.PhoneNumber).HasColumnName("phone_number");
            builder.Property(e => e.ImageUrl).HasColumnName("image_url");
            builder.Property(e => e.DistinguishedName).HasColumnName("distinguished_name");
            builder.Property(e => e.Sid).HasColumnName("sid");
            builder.Property(e => e.IsActive).HasColumnName("is_active");
            builder.Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Property(e => e.CreatedBy).HasColumnName("created_by");
            builder.Property(e => e.ModifiedAt).HasColumnName("modified_at");
            builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            builder.Property(e => e.DepartmentId).HasColumnName("department_id");
            builder.Property(e => e.PositionId).HasColumnName("position_id");
            builder.Property(e => e.LastLoginAt).HasColumnName("last_login_at");
            builder.Property(e => e.LastSyncAt).HasColumnName("last_sync_at");

            // ✅ BỔ SUNG thiếu
            builder.Property(e => e.Gender).HasColumnName("gender");
            builder.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            builder.Property(e => e.Address)
                .HasColumnName("address")
                .HasMaxLength(500);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(u => u.Username);

            builder.Property(u => u.PasswordHash)
                .HasMaxLength(255);

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(u => u.Email);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(u => u.DistinguishedName)
                .HasMaxLength(500);

            builder.Property(u => u.Sid)
                .HasMaxLength(100);

            builder.HasIndex(u => u.Sid);

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(u => u.ModifiedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(u => u.IsDeleted)
                .HasDefaultValue(false);

            builder.HasIndex(u => u.IsDeleted);

            builder.HasOne(u => u.Department)
                .WithMany()
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(u => u.Position)
                .WithMany()
                .HasForeignKey(u => u.PositionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}