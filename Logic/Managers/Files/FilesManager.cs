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

/// <summary>
/// file manager
/// </summary>
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

    /// <summary>
    /// method for searching for a user by token
    /// </summary>
    /// <param name="token">user access token</param>
    /// <returns>found userDal</returns>
    private async Task<UserDal> FindUser(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        if (jwt.ValidTo < DateTime.UtcNow) return null;
        var email = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        return await _userManager.FindByEmailAsync(email);
    }

    /// <summary>
    /// method for uploading files to yandex disk
    /// </summary>
    /// <param name="userId">unique userDal id</param>
    /// <param name="dalId">unique fileDal id</param>
    /// <param name="file">downloadable file</param>
    /// <param name="type">type file</param>
    private async Task UploadToCloudAsync(string userId, string dalId, IFormFile file, string type)
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
        
        await api.Files.UploadAsync(link, file.OpenReadStream());
        
    }
    
    /// <summary>
    /// method for uploading files to yandex disk
    /// </summary>
    /// <param name="token">user access token</param>
    /// <param name="file">downloadable file</param>
    /// <param name="dal">the entity(dal) with which we interact</param>
    /// <returns></returns>
    public async Task UploadFileAsync(string token, IFormFile file, FilesDal dal)
    {
        var user = await FindUser(token);
        dal.UserDal = user;
        var type = dal.Name.Split(".")[1];
        await _filesRepository.InsertAsync(dal);
        
        await UploadToCloudAsync(user.Id, dal.Id.ToString(), file, $".{type}");
    }

    /// <summary>
    /// method for uploading text file to yandex disk
    /// </summary>
    /// <param name="token">user access token</param>
    /// <param name="text">the text that we received</param>
    /// <param name="dal">the entity(dal) with which we interact</param>
    /// <returns></returns>
    public async Task UploadTextAsync(string token, string text, FilesDal dal)
    {
        var user = await FindUser(token);
        dal.UserDal = user;
        await _filesRepository.InsertAsync(dal);
        byte[] info = new UTF8Encoding(true).GetBytes(text);
        var stream = new MemoryStream(info);
        IFormFile file = new FormFile(stream, 0, info.Length, $"{dal.Name}", $"{dal.Name}");
        
        await UploadToCloudAsync(user.Id, dal.Id.ToString(), file, ".txt");
    }

    /// <summary>
    /// method for downloading a file from yandex disk
    /// </summary>
    /// <param name="dal">the entity(dal) with which we interact</param>
    /// <returns>we return everything necessary to assemble the user's file(FileStream, fileType, fileName)</returns>
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

    /// <summary>
    /// a method for searching for files that the user has on yandex disk
    /// </summary>
    /// <param name="token">user access token</param>
    /// <returns>returns a list of files that the user has on yandex disk</returns>
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

    /// <summary>
    /// method to delete a user's file from yandex disk
    /// </summary>
    /// <param name="fileId">unique fileDal id</param>
    /// <returns></returns>
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