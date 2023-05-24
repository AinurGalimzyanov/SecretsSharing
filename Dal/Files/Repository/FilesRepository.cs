using Dal.Base.Repository;
using Dal.Files.Entity;
using Dal.Files.Repository.Interface;

namespace Dal.Files.Repository;

public class FilesRepository : BaseRepository<FilesDal, Guid>, IFilesRepository
{
    private readonly DataContext _context;
    
    public FilesRepository(DataContext context) : base(context)
    {
        _context = context;
    }
}