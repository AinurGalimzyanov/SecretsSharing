using Dal.Base.Repository.Interface;
using Dal.Files.Entity;
using Dal.User.Entity;

namespace Dal.Files.Repository.Interface;

public interface IFilesRepository : IBaseRepository<FilesDal, Guid>
{
    public Task<UserDal> GetUserFile(Guid fileId);
}