using Microsoft.EntityFrameworkCore;

namespace ASP12_RazorPage_EntityFramework.Models;

public class MasterDbContext : DbContext
{
    public DbSet<Article>? Articles { get; set; }
    
    public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
    }
    
    protected override void OnModelCreating(ModelBuilder modelbuilder)
    {
        modelbuilder.ApplyUtcDateTimeConverter();//Put before seed data and after model creation
        base.OnModelCreating(modelbuilder);

    }

}