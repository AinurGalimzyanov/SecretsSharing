using Dal.Base.Entity;
using Dal.User.Entity;

namespace Dal.Files.Entity;

/// /// <summary>
/// file entity class
/// </summary>
public class FilesDal : BaseDal<Guid>
{
    /// <summary>
    /// file name
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// a value that determines whether a file should be deleted after it is download
    /// </summary>
    public bool Cascade { get; set; }
    
    /// <summary>
    /// relationship between the Users and File table
    /// </summary>
    public UserDal? UserDal { get; set; }
}