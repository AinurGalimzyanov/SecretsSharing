using Dal.Files.Entity;
using Microsoft.AspNetCore.Identity;

namespace Dal.User.Entity;

public class UserDal : IdentityUser
{
    /// <summary>
    /// refreshToken
    /// </summary>
    public string? RefreshToken { get; set; }
    
    /// <summary>
    /// relationship between the Users and File table
    /// </summary>
    public List<FilesDal>? FilesList { get; set; }
}