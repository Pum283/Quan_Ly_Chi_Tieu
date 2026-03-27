using Domain.Common;

namespace Domain.Entity
{
    public class Tag : BaseAuditableEntity, ISoftDelete
    {
        public int? WorkspaceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ColorToken { get; set; }
        public bool IsSystem { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public virtual Workspace? Workspace { get; set; }
    }
}
