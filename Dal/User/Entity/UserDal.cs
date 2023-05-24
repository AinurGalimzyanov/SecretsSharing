using Dal.Files.Entity;
using Microsoft.AspNetCore.Identity;

namespace Dal.User.Entity;

public class UserDal : IdentityUser
{
    public List<FilesDal>? FilesList { get; set; }
}