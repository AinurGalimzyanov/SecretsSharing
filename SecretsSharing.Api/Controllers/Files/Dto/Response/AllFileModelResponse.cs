using System.ComponentModel.DataAnnotations;
using Dal.Files.Entity;
using Newtonsoft.Json;

namespace SecretsSharing.Controllers.Files.Dto.Response;

/// <summary>
/// response model with information about all user files
/// </summary>
public class AllFileModelResponse
{
    /// <summary>
    /// list user files
    /// </summary>
    [Required] 
    [JsonProperty("Files")] 
    public List<FileModelResponse> Files { get; init; }

    public AllFileModelResponse(List<FileModelResponse> files)
    {
        Files = files;
    }
}