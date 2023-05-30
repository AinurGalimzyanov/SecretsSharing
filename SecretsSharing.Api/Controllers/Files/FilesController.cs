using Api.Controllers.Files.Dto.Request;
using AutoMapper;
using Dal.Files.Entity;
using Logic.Managers.Files.Interface;
using Microsoft.AspNetCore.Mvc;
using SecretsSharing.Controllers.Base;
using SecretsSharing.Controllers.Files.Dto.Response;

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

    [HttpPost("uploadFile")]
    public async Task<IActionResult> UploadFile(IFormFile? file, [FromQuery] bool сascade)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            var dal = new FilesDal() {Cascade = сascade, Name = file.FileName};
            await _filesManager.UploadFileAsync(token, file, dal);
            var uri = new Uri($"{Request.Scheme}://{Request.Host}/api/v1/Files/id={dal.Id}");
            return Ok(uri);
        }
        catch (Exception e)
        {
            return BadRequest(new { e.Message });
        }
    }
    
    [HttpPost("uploadText")]
    public async Task<IActionResult> UploadText([FromQuery] TextModelRequest model)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            var dal = new FilesDal() {Cascade = model.Cascade, Name = model.Name + ".txt"};
            await _filesManager.UploadTextAsync(token, model.Text, dal);
            var uri = new Uri($"{Request.Scheme}://{Request.Host}/api/v1/Files/id={dal.Id}");
            return Ok(uri);
        }
        catch (Exception e)
        {
            return BadRequest(new { e.Message });
        }
    }

    [HttpGet("id={id:guid}")]
    public async Task<IActionResult> DownloadFile(Guid id)
    {
        var dal = await _filesManager.GetAsync(id);
        var result = await _filesManager.DownloadFileAsync(dal);
        return File(result.Item1, result.Item2, result.Item3);
    }
    
    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteFile([FromRoute] Guid id)
    {
        try
        {
            await _filesManager.DeleteFileAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { e.Message });
        }
    }
    
    [HttpGet("getAllFile")]
    public async Task<IActionResult> GtAllFile()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var files = await _filesManager.GetAllFileAsync(token);
        var result = files.Select(x => _mapper.Map<FileModelResponse>(x)).ToList();
        return Ok(new AllFileModelResponse(result));
    }
}