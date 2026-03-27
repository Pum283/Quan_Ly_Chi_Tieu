using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.ToTable("Budgets");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.BudgetScopeType).IsRequired();
            builder.Property(x => x.PeriodType).IsRequired();
            builder.Property(x => x.PlannedAmount).HasPrecision(19, 4);

            builder.HasOne(x => x.Workspace)
                .WithMany()
                .HasForeignKey(x => x.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Wallet)
                .WithMany()
                .HasForeignKey(x => x.WalletId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => new { x.WorkspaceId, x.PeriodStart, x.PeriodEnd, x.IsActive });
        }
    }
}
