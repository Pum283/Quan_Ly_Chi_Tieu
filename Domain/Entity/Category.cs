using Domain.Common;
using Domain.Enum;

namespace Domain.Entity
{
    public class Category : BaseAuditableEntity, ISoftDelete
    {
        public int? WorkspaceId { get; set; }
        public CategoryType CategoryType { get; set; } = CategoryType.Expense;
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? Icon { get; set; }
        public string? ColorToken { get; set; }
        public bool IsSystem { get; set; }
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public virtual Workspace? Workspace { get; set; }
        public virtual Category? ParentCategory { get; set; }
        public virtual ICollection<Category> Children { get; set; } = new List<Category>();
    }
}
