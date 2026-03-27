using Domain.Common;

namespace Domain.Entity
{
    public class DebtPayment : BaseAuditableEntity, ISoftDelete
    {
        public int DebtId { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int? WalletId { get; set; }
        public int? TransactionId { get; set; }
        public string? Note { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public virtual Debt? Debt { get; set; }
        public virtual Wallet? Wallet { get; set; }
        public virtual Transaction? Transaction { get; set; }
    }
}
