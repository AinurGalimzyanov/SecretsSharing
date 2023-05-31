using Dal.Base.Repository.Interface;
using Dal.Files.Entity;
using Dal.User.Entity;

namespace Dal.Files.Repository.Interface;

/// <summary>
/// interface repository for working with files
/// </summary>
public interface IFilesRepository : IBaseRepository<FilesDal, Guid>
{
    /// <summary>
    /// get the user to whom a certain file belongs
    /// </summary>
    /// <param name="fileId">unique identifier of the FileDal</param>
    /// <returns>get UserDal</returns>
    public Task<UserDal> GetUserFile(Guid fileId);
}