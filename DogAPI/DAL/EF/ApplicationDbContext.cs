using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class ApplicationDbContext : DbContext
{
    public DbSet<Dog> Dogs { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}