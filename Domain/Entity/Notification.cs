using Domain.Common;
using Domain.Enum;

namespace Domain.Entity
{
    public class Notification : BaseAuditableEntity, ISoftDelete
    {
        public int? WorkspaceId { get; set; }
        public int UserId { get; set; }
        public NotificationChannel Channel { get; set; } = NotificationChannel.InApp;
        public NotificationType Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public string? ReferenceType { get; set; }
        public string? ReferenceId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public virtual Workspace? Workspace { get; set; }
        public virtual User? User { get; set; }
    }
}
