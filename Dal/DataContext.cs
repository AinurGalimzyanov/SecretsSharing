using Dal.Files.Entity;
using Dal.User.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dal;

/// <summary>
/// a class representing the current database view using Identity
/// </summary>
public class DataContext : IdentityDbContext<UserDal>
{
    /// <summary>
    /// list of all file table entries
    /// </summary>
    public DbSet<FilesDal> Files { get; set; }
    
    /// <summary>
    /// sends a request to update the database corresponding to the class fields
    /// </summary>
    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    /// <summary>
    /// constructor class, inherited from the base
    /// </summary>
    /// <param name="options">input parameters</param>
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
        Database.EnsureCreated();   
    }

    /// <summary>
    /// method for configuring database table fields
    /// </summary>
    /// <param name="modelBuilder">form of entities</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<UserDal>(entity => entity.ToTable(name: "Users"));
        builder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));
        builder.Entity<IdentityUserRole<string>>(entity =>
            entity.ToTable(name: "UserRoles"));
        builder.Entity<IdentityUserClaim<string>>(entity =>
            entity.ToTable(name: "UserClaim"));
        builder.Entity<IdentityUserLogin<string>>(entity =>
            entity.ToTable("UserLogins"));
        builder.Entity<IdentityUserToken<string>>(entity =>
            entity.ToTable("UserTokens"));
        builder.Entity<IdentityRoleClaim<string>>(entity =>
            entity.ToTable("RoleClaims"));
        
        builder.ApplyConfiguration(new AuthConfiguration());
        
    }
}