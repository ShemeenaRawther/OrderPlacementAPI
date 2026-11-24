using Microsoft.EntityFrameworkCore;
using OrderPlacementAPI.Model;

namespace OrderPlacementAPI.DBContext
{
    public class OrderDBContext : DbContext
    {
        public OrderDBContext(DbContextOptions<OrderDBContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<OrderDetails> OrderDetails => Set<OrderDetails>();
        public DbSet<IdempotencyKey> IdempotencyKeys => Set<IdempotencyKey>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Wireless Mouse", Price = 799, Stock = 50 },
            new Product { Id = 2, Name = "Mechanical Keyboard", Price = 3499, Stock = 20 },
            new Product { Id = 3, Name = "Limited Edition Headphones", Price = 9999, Stock = 1 }
            );

            modelBuilder.Entity<Customer>().HasData(            
            new Customer { Id = 1, Name = "Rahul Sharma" },
            new Customer { Id = 2, Name = "Priya Mehta" },
            new Customer { Id = 3, Name = "Amit Verma" }
            );
        }
    }
}
