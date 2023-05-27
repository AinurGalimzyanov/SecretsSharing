using Dal.Base.Repository;
using Dal.Files.Entity;
using Dal.Files.Repository.Interface;
using Dal.User.Entity;
using Microsoft.EntityFrameworkCore;

namespace Dal.Files.Repository;

public class FilesRepository : BaseRepository<FilesDal, Guid>, IFilesRepository
{
    private readonly DataContext _context;
    
    public FilesRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserDal> GetUserFile()
    {
        var file = await _context.Set<FilesDal>()
            .Include(x => x.UserDal)
            .FirstOrDefaultAsync();
        return file.UserDal;
    }
}