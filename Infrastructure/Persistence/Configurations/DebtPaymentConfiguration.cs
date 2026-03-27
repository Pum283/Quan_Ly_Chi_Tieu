using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class DebtPaymentConfiguration : IEntityTypeConfiguration<DebtPayment>
    {
        public void Configure(EntityTypeBuilder<DebtPayment> builder)
        {
            builder.ToTable("DebtPayments", tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("CK_DebtPayments_Amount_Positive", "[PaymentAmount] > 0");
            });
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PaymentAmount).HasPrecision(19, 4);
            builder.Property(x => x.Note).HasMaxLength(1000);

            builder.HasOne(x => x.Debt)
                .WithMany(x => x.DebtPayments)
                .HasForeignKey(x => x.DebtId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Wallet)
                .WithMany()
                .HasForeignKey(x => x.WalletId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Transaction)
                .WithMany()
                .HasForeignKey(x => x.TransactionId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
