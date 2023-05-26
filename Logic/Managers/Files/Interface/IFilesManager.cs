using Dal.Files.Entity;
using Logic.Managers.Base.Interface;

namespace Logic.Managers.Files.Interface;

public interface IFilesManager : IBaseManager<FilesDal, Guid>
{
    
}