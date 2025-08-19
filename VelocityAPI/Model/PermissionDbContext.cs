using Microsoft.EntityFrameworkCore;

namespace VelocityAPI.Model
{
    public class PermissionDbContext : DbContext
    {
        public PermissionDbContext(DbContextOptions<PermissionDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Page> Pages { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Page>().ToTable("Page");
            modelBuilder.Entity<Permission>().ToTable("Permission");

            modelBuilder.Entity<Permission>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Permission>()
                .HasOne(p => p.Page)
                .WithMany()
                .HasForeignKey(p => p.PageId);
        }
    }
}

