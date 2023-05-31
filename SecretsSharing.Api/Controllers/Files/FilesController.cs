using AutoMapper;
using Dal.Files.Entity;
using Logic.Managers.Files.Interface;
using Microsoft.AspNetCore.Mvc;
using SecretsSharing.Controllers.Base;
using SecretsSharing.Controllers.Files.Dto.Request;
using SecretsSharing.Controllers.Files.Dto.Response;

namespace SecretsSharing.Controllers.Files;

/// <summary>
/// controller that works with requests related to files
/// </summary>
public class FilesController : BaseController
{
    private readonly IFilesManager _filesManager;
    private readonly IMapper _mapper;
    private string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ1c2VyQGV4YW1wbGUuY29tIiwibmJmIjoxNjg1NTU1NDg2LCJleHAiOjE2ODU1NTYzODYsImlzcyI6ImZhbHNlIiwiYXVkIjoic2RmZiJ9.rEYyCxGY-GKqSo9iAkxfYShgCvyUoU5vUjpMLh0cuF0";
    
    /// <summary>
    /// controller constructor
    /// </summary>
    /// <param name="filesManager">file logic service</param>
    /// <param name="mapper">automapper</param>
    public FilesController(IFilesManager filesManager, IMapper mapper)
    {
        _filesManager = filesManager;
        _mapper = mapper;
    }

    /// <summary>
    /// route for uploading files to yandex disk
    /// </summary>
    /// <param name="file">downloadable file</param>
    /// <param name="сascade">a value that determines whether a file should be deleted after it is download</param>
    /// <returns>the uri by which it will be possible to download the file from yandex disk</returns>
    [HttpPost("uploadFile")]
    public async Task<IActionResult> UploadFile(IFormFile? file, [FromQuery] bool сascade)
    {
        try
        {
            //var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
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
    
    /// <summary>
    /// route for uploading text files to yandex disk
    /// </summary>
    /// <param name="model">the model of information about the file</param>
    /// <returns>the uri by which it will be possible to download the file from yandex disk</returns>
    [HttpPost("uploadText")]
    public async Task<IActionResult> UploadText([FromQuery] TextModelRequest model)
    {
        try
        {
            //var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
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

    /// <summary>
    /// route by which it will be possible to download the file from yandex disk
    /// </summary>
    /// <param name="id">unique fileDal id</param>
    /// <returns>file</returns>
    [HttpGet("id={id:guid}")]
    public async Task<IActionResult> DownloadFile(Guid id)
    {
        var dal = await _filesManager.GetAsync(id);
        var result = await _filesManager.DownloadFileAsync(dal);
        return File(result.Item1, result.Item2, result.Item3);
    }
    
    /// <summary>
    /// the route by which you can delete a file from yandex disk
    /// </summary>
    /// <param name="id">unique fileDal id</param>
    /// <returns></returns>
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
    
    /// <summary>
    /// the route by which you can get a list of all the user's files
    /// </summary>
    /// <returns>list of all user files</returns>
    [HttpGet("getAllFile")]
    public async Task<IActionResult> GtAllFile()
    {
        //var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var files = await _filesManager.GetAllFileAsync(token);
        var result = files.Select(x => _mapper.Map<FileModelResponse>(x)).ToList();
        return Ok(new AllFileModelResponse(result));
    }
}