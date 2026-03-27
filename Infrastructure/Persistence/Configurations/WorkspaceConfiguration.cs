using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
    {
        public void Configure(EntityTypeBuilder<Workspace> builder)
        {
            builder.ToTable("Workspaces");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.DefaultCurrencyCode).IsRequired().HasMaxLength(10);

            builder.HasOne(x => x.OwnerUser)
                .WithMany(x => x.OwnedWorkspaces)
                .HasForeignKey(x => x.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.OwnerUserId, x.Name });
        }
    }
}
