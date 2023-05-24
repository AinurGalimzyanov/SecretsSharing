using Dal.Base.Repository.Interface;
using Dal.Files.Entity;

namespace Dal.Files.Repository.Interface;

public interface IFilesRepository : IBaseRepository<FilesDal, Guid>
{
    
}