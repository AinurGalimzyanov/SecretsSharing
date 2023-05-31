using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SecretsSharing.Controllers.Files.Dto.Response;

/// <summary>
/// response model with inforation about the user file 
/// </summary>
public class FileModelResponse
{
    /// <summary>
    /// file name
    /// </summary>
    [Required] 
    [JsonProperty("Name")] 
    public string Name { get; init; }

    public FileModelResponse(string name)
    {
        Name = name;
    }
}