using Domain.Common;
using Domain.Enum;

namespace Domain.Entity
{
    public class Workspace : BaseAuditableEntity, ISoftDelete
    {
        public string Name { get; set; } = string.Empty;
        public WorkspaceType Type { get; set; } = WorkspaceType.Personal;
        public string DefaultCurrencyCode { get; set; } = "VND";
        public bool RequireApproval { get; set; }
        public int OwnerUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public virtual User? OwnerUser { get; set; }
        public virtual ICollection<WorkspaceMember> Members { get; set; } = new List<WorkspaceMember>();
        public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
