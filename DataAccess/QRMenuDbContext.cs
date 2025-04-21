using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class QRMenuDbContext : DbContext
    {
        public QRMenuDbContext(DbContextOptions<QRMenuDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TableItem> TableItems { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<QRToken> QRTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Decimal precision settings
            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
               .Property(oi => oi.Price)
               .HasPrecision(18, 2);

            // Table configurations
            modelBuilder.Entity<Table>(entity =>
            {
                entity.HasKey(t => t.Id);  // Primary key açıkça belirtiliyor

                entity.HasMany(t => t.Orders)
                    .WithOne(o => o.Table)
                    .HasForeignKey(o => o.TableId)
                    .HasPrincipalKey(t => t.Id)  // Principal key belirtiliyor
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.TableItems)
                    .WithOne(ti => ti.Table)
                    .HasForeignKey(ti => ti.TableId)
                    .HasPrincipalKey(t => t.Id)  // Principal key belirtiliyor
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // TableItem configurations
            modelBuilder.Entity<TableItem>(entity =>
            {
                entity.HasKey(ti => ti.Id);

                entity.HasOne(ti => ti.Table)
                    .WithMany(t => t.TableItems)
                    .HasForeignKey(ti => ti.TableId)
                    .HasPrincipalKey(t => t.Id)  // Table.Id ile ilişkilendirme
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ti => ti.MenuItem)
                    .WithMany(mi => mi.TableItems)
                    .HasForeignKey(ti => ti.MenuItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Order configurations
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.HasOne(o => o.Table)
                    .WithMany(t => t.Orders)
                    .HasForeignKey(o => o.TableId)
                    .HasPrincipalKey(t => t.Id)  // Table.Id ile ilişkilendirme
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.OrderItems)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(oi => oi.OrderId)
                    .HasPrincipalKey(o => o.Id)  // Order.Id ile ilişkilendirme
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // OrderItem configurations
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);

                entity.HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId)
                    .HasPrincipalKey(o => o.Id)  // Order.Id ile ilişkilendirme
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(oi => oi.MenuItem)
                    .WithMany(mi => mi.OrderItems)
                    .HasForeignKey(oi => oi.MenuItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // UserRole composite key
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

              
                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Auto-include related data configurations
            modelBuilder.Entity<Order>()
                .Navigation(o => o.Table).AutoInclude();

            modelBuilder.Entity<Table>()
                .Navigation(t => t.TableItems).AutoInclude();

            modelBuilder.Entity<TableItem>()
                .Navigation(ti => ti.MenuItem).AutoInclude();

            modelBuilder.Entity<Order>()
                .Navigation(o => o.OrderItems).AutoInclude();

            modelBuilder.Entity<OrderItem>()
                .Navigation(oi => oi.MenuItem).AutoInclude();
        }
    }
}