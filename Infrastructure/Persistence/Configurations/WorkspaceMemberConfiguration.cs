using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class WorkspaceMemberConfiguration : IEntityTypeConfiguration<WorkspaceMember>
    {
        public void Configure(EntityTypeBuilder<WorkspaceMember> builder)
        {
            builder.ToTable("WorkspaceMembers");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Role).IsRequired();
            builder.Property(x => x.Status).IsRequired();

            builder.HasOne(x => x.Workspace)
                .WithMany(x => x.Members)
                .HasForeignKey(x => x.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany(x => x.WorkspaceMembers)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.WorkspaceId, x.UserId }).IsUnique();
        }
    }
}
