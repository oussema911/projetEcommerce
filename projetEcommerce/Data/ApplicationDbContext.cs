namespace projetEcommerce.Data
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using projetEcommerce.Models; // Assurez-vous que ce namespace est correct


    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        //public DbSet<OrderItem> OrderItems { get; set; }  // Assurez-vous que cette ligne existe

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");
        }

    }

}
