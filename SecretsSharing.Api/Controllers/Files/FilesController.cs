using Api.Controllers.Files.Dto.Request;
using AutoMapper;
using Logic.Managers.Files.Interface;
using Microsoft.AspNetCore.Mvc;
using SecretsSharing.Controllers.Base;
using YandexDisk.Client.Http;

namespace SecretsSharing.Controllers.Files;

public class FilesController : BaseController
{
    private readonly IFilesManager _filesManager;
    private readonly IMapper _mapper;

    public FilesController(IFilesManager filesManager, IMapper mapper)
    {
        _filesManager = filesManager;
        _mapper = mapper;
    }

    [HttpPost("addFile")]
    public async Task<IActionResult> AddFile(IFormFile uploadedImg)
    {
        var api = new DiskHttpApi("y0_AgAAAABR93eBAAn3FAAAAADj_iQ-f3UvbKf6QkOvsUWylH2gEL66jvU");
        var link = await api.Files.GetUploadLinkAsync("/" + "SecretsSharing" + "/" + Path.GetFileName(uploadedImg.FileName),
            overwrite: false);
        var path = @"E:\JetBrains Rider 2022.2.2\SecretsSharing\Dal\wwwroot\Files\" + uploadedImg.FileName;
        
        using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
        {
            await uploadedImg.CopyToAsync(fileStream);
        }
        
        using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
        {
            await api.Files.UploadAsync(link, fileStream);
        }
        
        return Ok();
    }

    
}