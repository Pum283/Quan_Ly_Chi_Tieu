using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class WorkspaceInvitationConfiguration : IEntityTypeConfiguration<WorkspaceInvitation>
    {
        public void Configure(EntityTypeBuilder<WorkspaceInvitation> builder)
        {
            builder.ToTable("WorkspaceInvitations");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email).IsRequired().HasMaxLength(256);
            builder.Property(x => x.InviteTokenHash).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Role).IsRequired();

            builder.HasOne(x => x.Workspace)
                .WithMany()
                .HasForeignKey(x => x.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.WorkspaceId, x.Email, x.ExpiresAt });
        }
    }
}
