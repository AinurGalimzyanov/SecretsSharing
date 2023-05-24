using Dal.Files.Entity;
using Dal.User.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dal;

public class DataContext : IdentityDbContext<UserDal>
{
    public DbSet<FilesDal> Files { get; set; }
    
    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
        Database.EnsureCreated();   
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserDal>(entity => entity.ToTable(name: "Users"));
        builder.ApplyConfiguration(new AuthConfiguration());
        
    }
}