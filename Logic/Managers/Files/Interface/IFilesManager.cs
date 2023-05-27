using Dal.Files.Entity;
using Logic.Managers.Base.Interface;
using Microsoft.AspNetCore.Http;

namespace Logic.Managers.Files.Interface;

public interface IFilesManager : IBaseManager<FilesDal, Guid>
{
    public Task UploadFileAsync(string token, IFormFile file, FilesDal dal);
    public Task UploadTextAsync(string token, string text, FilesDal dal);
    public Task<Tuple<Stream, string, string>> DownloadFileAsync(FilesDal dal);
    public Task<List<FilesDal>> GetAllFileAsync(string token);
}