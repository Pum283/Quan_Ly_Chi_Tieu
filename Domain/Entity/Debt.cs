using Domain.Common;
using Domain.Enum;

namespace Domain.Entity
{
    public class Debt : BaseAuditableEntity, ISoftDelete
    {
        public int WorkspaceId { get; set; }
        public DebtType DebtType { get; set; }
        public string CounterpartyName { get; set; } = string.Empty;
        public string? CounterpartyContact { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string CurrencyCode { get; set; } = "VND";
        public DateTime? DueDate { get; set; }
        public DebtStatus Status { get; set; } = DebtStatus.NotDue;
        public string? Note { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public virtual Workspace? Workspace { get; set; }
        public virtual ICollection<DebtPayment> DebtPayments { get; set; } = new List<DebtPayment>();
    }
}
