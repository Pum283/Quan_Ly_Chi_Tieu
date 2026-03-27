using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email).IsRequired().HasMaxLength(256);
            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(500);
            builder.Property(x => x.DisplayName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.SystemRole).IsRequired();

            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
