using Domain.Common;
using Domain.Enum;

namespace Domain.Entity
{
    public class Wallet : BaseAuditableEntity, ISoftDelete
    {
        public int WorkspaceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public WalletType WalletType { get; set; } = WalletType.Cash;
        public string CurrencyCode { get; set; } = "VND";
        public decimal OpeningBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public bool IsLocked { get; set; }
        public bool IsSystem { get; set; }
        public int SortOrder { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public virtual Workspace? Workspace { get; set; }
    }
}
