using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Files.Dto.Request;

public class FileModelRequest
{
    
    [JsonProperty("Text")]
    public string? Text { get; init; }
    
    [JsonProperty("isDelete")]
    public bool IsDelete { get; init; }
}