namespace ProtonDbApi.Repos;
using Microsoft.EntityFrameworkCore;
using Models;

public class ReportContext : DbContext
{
    public DbSet<Reports> Reports { get; set; }

    public ReportContext(DbContextOptions<ReportContext> options) : base(options)
    {}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reports>()
            .HasOne(p => p.Notes);
    }
    
}
