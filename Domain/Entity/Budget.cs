using Domain.Common;
using Domain.Enum;

namespace Domain.Entity
{
    public class Budget : BaseAuditableEntity, ISoftDelete
    {
        public int WorkspaceId { get; set; }
        public BudgetScopeType BudgetScopeType { get; set; } = BudgetScopeType.Category;
        public int? CategoryId { get; set; }
        public int? WalletId { get; set; }
        public int? MemberId { get; set; }
        public PeriodType PeriodType { get; set; } = PeriodType.Monthly;
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal PlannedAmount { get; set; }
        public bool WarningThreshold80 { get; set; } = true;
        public bool WarningThreshold100 { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public virtual Workspace? Workspace { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
