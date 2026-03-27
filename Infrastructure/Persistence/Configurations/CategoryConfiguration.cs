using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CategoryType).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Code).HasMaxLength(100);
            builder.Property(x => x.Icon).HasMaxLength(100);
            builder.Property(x => x.ColorToken).HasMaxLength(100);

            builder.HasOne(x => x.Workspace)
                .WithMany(x => x.Categories)
                .HasForeignKey(x => x.WorkspaceId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.ParentCategory)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => new { x.WorkspaceId, x.CategoryType, x.Name }).IsUnique();
        }
    }
}
