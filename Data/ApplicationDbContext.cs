using Microsoft.EntityFrameworkCore;
using ShoppingMVC.Models;

namespace ShoppingMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Login>Users { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<Cart>Cart{ get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }
    }
}
