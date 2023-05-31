using Dal.User.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dal;

/// <summary>
/// class configuration for authorization
/// </summary>
public class AuthConfiguration : IEntityTypeConfiguration<UserDal>
{
    public void Configure(EntityTypeBuilder<UserDal> builder)
    {
        builder.HasKey(x => x.Id);
    }
}