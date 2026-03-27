using Domain.Common;
using Domain.Enum;

namespace Domain.Entity
{
    public class WorkspaceInvitation : BaseAuditableEntity, ISoftDelete
    {
        public int WorkspaceId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string InviteTokenHash { get; set; } = string.Empty;
        public WorkspaceRole Role { get; set; } = WorkspaceRole.Member;
        public DateTime ExpiresAt { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public virtual Workspace? Workspace { get; set; }
    }
}
