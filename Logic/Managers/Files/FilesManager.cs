using Dal.Base.Repository.Interface;
using Dal.Files.Entity;
using Dal.Files.Repository.Interface;
using Dal.User.Entity;
using Logic.Managers.Base;
using Logic.Managers.Files.Interface;
using Microsoft.AspNetCore.Identity;

namespace Logic.Managers.Files;

public class FilesManager  : BaseManager<FilesDal, Guid>, IFilesManager
{
    private readonly UserManager<UserDal> _userManager;
    private readonly IFilesRepository _filesRepository;


    public FilesManager(IFilesRepository repository, UserManager<UserDal> userManager)
        : base(repository)
    {
        _userManager = userManager;
        _filesRepository = repository;
    }
    
}