using Dal.Base.Entity;
using Dal.User.Entity;

namespace Dal.Files.Entity;

public class FilesDal : BaseDal<Guid>
{
    public string? Name { get; set; }
    
    public bool Cascade { get; set; }
    
    public UserDal? UserDal { get; set; }
}