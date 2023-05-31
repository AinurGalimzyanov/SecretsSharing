using Dal.Files.Entity;
using Logic.Managers.Base.Interface;
using Microsoft.AspNetCore.Http;

namespace Logic.Managers.Files.Interface;

/// <summary>
/// interface file manager
/// </summary>
public interface IFilesManager : IBaseManager<FilesDal, Guid>
{
    /// <summary>
    /// method for uploading files to yandex disk
    /// </summary>
    /// <param name="token">user access token</param>
    /// <param name="file">downloadable file</param>
    /// <param name="dal">the entity(dal) with which we interact</param>
    /// <returns></returns>
    public Task UploadFileAsync(string token, IFormFile file, FilesDal dal);
    
    /// <summary>
    /// method for uploading files to yandex disk
    /// </summary>
    /// <param name="token">user access token</param>
    /// <param name="text">the text that we received</param>
    /// <param name="dal">the entity(dal) with which we interact</param>
    /// <returns></returns>
    public Task UploadTextAsync(string token, string text, FilesDal dal);
    
    /// <summary>
    /// method for downloading a file from yandex disk
    /// </summary>
    /// <param name="dal">the entity(dal) with which we interact</param>
    /// <returns>we return everything necessary to assemble the user's file(FileStream, fileType, fileName)</returns>
    public Task<Tuple<Stream, string, string>> DownloadFileAsync(FilesDal dal);
    
    /// <summary>
    /// a method for searching for files that the user has on yandex disk
    /// </summary>
    /// <param name="token">user access token</param>
    /// <returns>returns a list of files that the user has on yandex disk</returns>
    public Task<List<FilesDal>> GetAllFileAsync(string token);
    
    /// <summary>
    /// method to delete a user's file from yandex disk
    /// </summary>
    /// <param name="fileId">unique fileDal id</param>
    /// <returns></returns>
    public Task DeleteFileAsync(Guid fileId);
}