using Microsoft.EntityFrameworkCore;
using Social_Models;

namespace Social_DataAccess;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Activity> Activities { get; set; }
}