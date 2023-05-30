using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dal.Base.Repository.Interface;
using Dal.Files.Entity;
using Dal.Files.Repository.Interface;
using Dal.User.Entity;
using Logic.Managers.Base;
using Logic.Managers.Files.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using YandexDisk.Client.Clients;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;

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

    private async Task<UserDal> FindUser(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        if (jwt.ValidTo < DateTime.UtcNow) return null;
        var email = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        return await _userManager.FindByEmailAsync(email);
    }

    private async Task UploadToCloudAsync(string userId, string dalId, string pathTemporaryStorageFile, string type)
    {
        var api = new DiskHttpApi("y0_AgAAAABR93eBAAn3FAAAAADj_iQ-f3UvbKf6QkOvsUWylH2gEL66jvU");
        var pathDirUser = "SecretsSharing" + "/" + userId;
        var roodFolderDate = await api.MetaInfo.GetInfoAsync(new ResourceRequest
        {
            Path = "/SecretsSharing"
        });

        if (!roodFolderDate.Embedded.Items.Any(i => i.Type == ResourceType.Dir && i.Name.Equals(userId)))
        {
            await api.Commands.CreateDictionaryAsync(pathDirUser);
        }
        
        var link = await api.Files.GetUploadLinkAsync("/" + pathDirUser + "/" + dalId + type,
            overwrite: false);
        
        using (var fileStream = new FileStream(pathTemporaryStorageFile, FileMode.OpenOrCreate))
        {
            await api.Files.UploadAsync(link, fileStream);
        }
        
        File.Delete(pathTemporaryStorageFile);
    }
    
    public async Task UploadFileAsync(string token, IFormFile file, FilesDal dal)
    {
        var user = await FindUser(token);
        dal.UserDal = user;
        var type = dal.Name.Split(".")[1];
        await _filesRepository.InsertAsync(dal);
        
        var pathTemporaryStorageFile = @"E:\JetBrains Rider 2022.2.2\SecretsSharing\Dal\wwwroot\Files\" + dal.Id;

        using (var fileStream = new FileStream(pathTemporaryStorageFile, FileMode.OpenOrCreate))
        {
            await file.CopyToAsync(fileStream);
        }

        await UploadToCloudAsync(user.Id, dal.Id.ToString(), pathTemporaryStorageFile, $".{type}");
    }

    public async Task UploadTextAsync(string token, string text, FilesDal dal)
    {
        var user = await FindUser(token);
        dal.UserDal = user;
        await _filesRepository.InsertAsync(dal);
        var pathTemporaryStorageFile = @"E:\JetBrains Rider 2022.2.2\SecretsSharing\Dal\wwwroot\Files\" + dal.Id;
        
        using (FileStream fs = File.Create(pathTemporaryStorageFile))
        {
            byte[] info = new UTF8Encoding(true).GetBytes(text);
            fs.Write(info, 0, info.Length);
        }
        
        await UploadToCloudAsync(user.Id, dal.Id.ToString(), pathTemporaryStorageFile, ".txt");
    }

    public async Task<Tuple<Stream, string, string>> DownloadFileAsync(FilesDal dal)
    {
        var user = await _filesRepository.GetUserFile(dal.Id);
        var type = dal.Name.Split(".")[1];
        var api = new DiskHttpApi("y0_AgAAAABR93eBAAn3FAAAAADj_iQ-f3UvbKf6QkOvsUWylH2gEL66jvU");
        var path = $"/SecretsSharing/{user.Id}/{dal.Id}.{type}";
        var fileStream = await api.Files.DownloadFileAsync(path);
        var fileType="application/octet-stream";
        if (dal.Cascade)
        {
            await api.Commands.DeleteAsync(new DeleteFileRequest() {Path = path});
            await _filesRepository.DeleteAsync(dal.Id);
        }
        return new Tuple<Stream, string, string>(fileStream, fileType, $"img.{type}");
    }

    public async Task<List<FilesDal>> GetAllFileAsync(string token)
    {
        var files = new List<FilesDal>();
        var user = await FindUser(token);
        var api = new DiskHttpApi("y0_AgAAAABR93eBAAn3FAAAAADj_iQ-f3UvbKf6QkOvsUWylH2gEL66jvU");
        var roodFolderDate = await api.MetaInfo.GetInfoAsync(new ResourceRequest
        {
            Path = $"/SecretsSharing/{user.Id}"
        });
        foreach (var item in roodFolderDate.Embedded.Items)
        {
            var id = item.Name.Split(".")[0];
            var guid = Guid.Parse(id);
            files.Add(await _filesRepository.GetAsync(guid));
        }

        return files;
    }

    public async Task DeleteFileAsync(Guid fileId)
    {
        var file = await GetAsync(fileId);
        var user = await _filesRepository.GetUserFile(file.Id);
        var type = file.Name.Split(".")[1];
        var api = new DiskHttpApi("y0_AgAAAABR93eBAAn3FAAAAADj_iQ-f3UvbKf6QkOvsUWylH2gEL66jvU");
        var path = $"/SecretsSharing/{user.Id}/{file.Id}.{type}";
        await api.Commands.DeleteAsync(new DeleteFileRequest() {Path = path});
        await DeleteAsync(fileId);
    }
}