using Microsoft.EntityFrameworkCore;
using Pharmacy.Models;

namespace Pharmacy;

public class ApplicationContext : DbContext
{
    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Category> Categorys { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Form> Forms { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<OrderBuyer> OrderBuyers { get; set; }
    public DbSet<PlaceOfIssue> PlaceOfIssues { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}