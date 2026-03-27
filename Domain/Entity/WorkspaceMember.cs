using Domain.Common;
using Domain.Enum;

namespace Domain.Entity
{
    public class WorkspaceMember : BaseAuditableEntity, ISoftDelete
    {
        public int WorkspaceId { get; set; }
        public int UserId { get; set; }
        public WorkspaceRole Role { get; set; } = WorkspaceRole.Member;
        public MembershipStatus Status { get; set; } = MembershipStatus.Invited;
        public DateTime? JoinedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public virtual Workspace? Workspace { get; set; }
        public virtual User? User { get; set; }
    }
}
