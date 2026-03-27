using Domain.Common;
using Domain.Enum;

namespace Domain.Entity
{
    public class Transaction : BaseAuditableEntity, ISoftDelete
    {
        public int WorkspaceId { get; set; }
        public string TransactionNo { get; set; } = string.Empty;
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; } = "VND";
        public decimal? ExchangeRate { get; set; }
        public decimal AmountInWorkspaceCurrency { get; set; }
        public DateTime OccurredAt { get; set; }
        public DateTime RecordedAt { get; set; }
        public int? SourceWalletId { get; set; }
        public int? DestinationWalletId { get; set; }
        public int? CategoryId { get; set; }
        public int? PayerMemberId { get; set; }
        public int? BeneficiaryMemberId { get; set; }
        public string? Note { get; set; }
        public TransactionStatus Status { get; set; } = TransactionStatus.Draft;
        public string? ExternalReference { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public virtual Workspace? Workspace { get; set; }
        public virtual Wallet? SourceWallet { get; set; }
        public virtual Wallet? DestinationWallet { get; set; }
        public virtual Category? Category { get; set; }
    }
}
