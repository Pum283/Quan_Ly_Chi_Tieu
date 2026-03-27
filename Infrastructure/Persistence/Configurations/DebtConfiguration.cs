using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class DebtConfiguration : IEntityTypeConfiguration<Debt>
    {
        public void Configure(EntityTypeBuilder<Debt> builder)
        {
            builder.ToTable("Debts");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.DebtType).IsRequired();
            builder.Property(x => x.CounterpartyName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.CounterpartyContact).HasMaxLength(200);
            builder.Property(x => x.PrincipalAmount).HasPrecision(19, 4);
            builder.Property(x => x.OutstandingAmount).HasPrecision(19, 4);
            builder.Property(x => x.CurrencyCode).IsRequired().HasMaxLength(10);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Note).HasMaxLength(1000);

            builder.HasOne(x => x.Workspace)
                .WithMany()
                .HasForeignKey(x => x.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.WorkspaceId, x.Status, x.DueDate });
        }
    }
}
