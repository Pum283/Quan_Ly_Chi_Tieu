using Domain.Common;
using Domain.Enum;

namespace Domain.Entity
{
    public class User : BaseAuditableEntity, ISoftDelete
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public UserStatus Status { get; set; } = UserStatus.Active;
        public SystemRole SystemRole { get; set; } = SystemRole.User;
        public DateTime? LastLoginAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public virtual ICollection<Workspace> OwnedWorkspaces { get; set; } = new List<Workspace>();
        public virtual ICollection<WorkspaceMember> WorkspaceMembers { get; set; } = new List<WorkspaceMember>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
