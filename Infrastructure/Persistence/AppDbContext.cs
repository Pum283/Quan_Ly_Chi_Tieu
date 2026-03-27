using Microsoft.EntityFrameworkCore;
using Domain.Common;
using Domain.Entity;
using System.Reflection;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Workspace> Workspaces => Set<Workspace>();
        public DbSet<WorkspaceMember> WorkspaceMembers => Set<WorkspaceMember>();
        public DbSet<WorkspaceInvitation> WorkspaceInvitations => Set<WorkspaceInvitation>();
        public DbSet<Wallet> Wallets => Set<Wallet>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<TransactionTagMapping> TransactionTagMappings => Set<TransactionTagMapping>();
        public DbSet<Debt> Debts => Set<Debt>();
        public DbSet<DebtPayment> DebtPayments => Set<DebtPayment>();
        public DbSet<Budget> Budgets => Set<Budget>();
        public DbSet<Notification> Notifications => Set<Notification>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Apply all configurations from assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            // Global query filters
            ConfigureGlobalFilters(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        private void ConfigureGlobalFilters(ModelBuilder modelBuilder)
        {
            // Apply soft delete filter
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                    var property = System.Linq.Expressions.Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                    var filter = System.Linq.Expressions.Expression.Lambda(
                        System.Linq.Expressions.Expression.Equal(property, System.Linq.Expressions.Expression.Constant(false)),
                        parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Set audit fields
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.UtcNow;
                        break;
                }
            }

            // Set soft delete fields
            foreach (var entry in ChangeTracker.Entries<ISoftDelete>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedDate = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
