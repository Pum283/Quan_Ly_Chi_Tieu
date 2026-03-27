using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions", tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("CK_Transactions_Amount_Positive", "[Amount] > 0");
                tableBuilder.HasCheckConstraint(
                    "CK_Transactions_Transfer_Wallets",
                    "([TransactionType] <> 3) OR ([SourceWalletId] IS NOT NULL AND [DestinationWalletId] IS NOT NULL AND [SourceWalletId] <> [DestinationWalletId])");
                tableBuilder.HasCheckConstraint(
                    "CK_Transactions_Category_Required_For_IncomeExpense",
                    "([TransactionType] NOT IN (1,2)) OR ([CategoryId] IS NOT NULL)");
            });
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TransactionNo).IsRequired().HasMaxLength(100);
            builder.Property(x => x.TransactionType).IsRequired();
            builder.Property(x => x.Amount).HasPrecision(19, 4);
            builder.Property(x => x.CurrencyCode).IsRequired().HasMaxLength(10);
            builder.Property(x => x.ExchangeRate).HasPrecision(19, 8);
            builder.Property(x => x.AmountInWorkspaceCurrency).HasPrecision(19, 4);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Note).HasMaxLength(1000);
            builder.Property(x => x.ExternalReference).HasMaxLength(200);

            builder.HasOne(x => x.Workspace)
                .WithMany()
                .HasForeignKey(x => x.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.SourceWallet)
                .WithMany()
                .HasForeignKey(x => x.SourceWalletId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.DestinationWallet)
                .WithMany()
                .HasForeignKey(x => x.DestinationWalletId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => new { x.WorkspaceId, x.TransactionNo }).IsUnique();
            builder.HasIndex(x => new { x.WorkspaceId, x.OccurredAt });
            builder.HasIndex(x => new { x.WorkspaceId, x.TransactionType, x.OccurredAt });
            builder.HasIndex(x => new { x.WorkspaceId, x.CategoryId, x.OccurredAt });
            builder.HasIndex(x => new { x.WorkspaceId, x.SourceWalletId, x.OccurredAt });
            builder.HasIndex(x => new { x.WorkspaceId, x.Status, x.OccurredAt });

        }
    }
}
