using Microsoft.EntityFrameworkCore;
 
namespace E_Commerce.Models
{
    public class ECommerceContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public ECommerceContext(DbContextOptions<ECommerceContext> options) : base(options) { }
        public DbSet<Product> products { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<Order> orders { get; set; }
    }
}