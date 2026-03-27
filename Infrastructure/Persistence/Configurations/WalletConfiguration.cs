using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallets");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.WalletType).IsRequired();
            builder.Property(x => x.CurrencyCode).IsRequired().HasMaxLength(10);
            builder.Property(x => x.OpeningBalance).HasPrecision(19, 4);
            builder.Property(x => x.CurrentBalance).HasPrecision(19, 4);

            builder.HasOne(x => x.Workspace)
                .WithMany(x => x.Wallets)
                .HasForeignKey(x => x.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.WorkspaceId, x.Name }).IsUnique();
        }
    }
}
