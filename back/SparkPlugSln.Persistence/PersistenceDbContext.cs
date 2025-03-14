using Microsoft.EntityFrameworkCore;
using SparkPlugSln.Domain.Entities;
using SparkPlugSln.Domain.Entities.Cart;
using SparkPlugSln.Domain.Entities.User;

namespace SparkPlugSln.Persistence;

public class PersistenceDbContext : DbContext
{
    public PersistenceDbContext(DbContextOptions<PersistenceDbContext> options) : base(options)
    {
        
    }

    #region User

    public DbSet<User> Users { get; set; }
    public DbSet<UserAddress> UsersAddresses { get; set; }

    #endregion

    #region Product

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductComment> ProductsComments { get; set; }
    public DbSet<ProductFile> ProductsFiles { get; set; }
    
    #endregion

    #region Cart

    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    #endregion
}