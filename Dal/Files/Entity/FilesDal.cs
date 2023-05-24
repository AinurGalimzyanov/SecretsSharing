using Dal.Base.Entity;
using Dal.User.Entity;

namespace Dal.Files.Entity;

public class FilesDal : BaseDal<Guid>
{
    public string? Path { get; set; }
    
    public UserDal? UserDal { get; set; }
}