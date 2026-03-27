using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Channel).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(300);
            builder.Property(x => x.Content).IsRequired().HasMaxLength(2000);
            builder.Property(x => x.ReferenceType).HasMaxLength(100);
            builder.Property(x => x.ReferenceId).HasMaxLength(100);

            builder.HasOne(x => x.Workspace)
                .WithMany()
                .HasForeignKey(x => x.WorkspaceId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.UserId, x.IsRead, x.SentAt });
        }
    }
}
