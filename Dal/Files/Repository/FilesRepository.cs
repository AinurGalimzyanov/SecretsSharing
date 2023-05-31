using Dal.Base.Repository;
using Dal.Files.Entity;
using Dal.Files.Repository.Interface;
using Dal.User.Entity;
using Microsoft.EntityFrameworkCore;

namespace Dal.Files.Repository;

/// <summary>
/// repository for working with files
/// </summary>
public class FilesRepository : BaseRepository<FilesDal, Guid>, IFilesRepository
{
    private readonly DataContext _context;
    
    public FilesRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// get the user to whom a certain file belongs
    /// </summary>
    /// <param name="fileId">unique identifier of the FileDal</param>
    /// <returns>get UserDal</returns>
    public async Task<UserDal> GetUserFile(Guid fileId)
    {
        var file = await _context.Set<FilesDal>()
            .Where(x => x.Id == fileId)
            .Include(x => x.UserDal)
            .FirstOrDefaultAsync();
        return file.UserDal;
    }
}