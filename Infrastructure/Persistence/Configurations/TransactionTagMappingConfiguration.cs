using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class TransactionTagMappingConfiguration : IEntityTypeConfiguration<TransactionTagMapping>
    {
        public void Configure(EntityTypeBuilder<TransactionTagMapping> builder)
        {
            builder.ToTable("TransactionTagMappings");
            builder.HasKey(x => new { x.TransactionId, x.TagId });

            builder.HasOne(x => x.Transaction)
                .WithMany()
                .HasForeignKey(x => x.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Tag)
                .WithMany()
                .HasForeignKey(x => x.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
