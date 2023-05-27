using System.ComponentModel.DataAnnotations;
using Dal.Files.Entity;
using Newtonsoft.Json;

namespace SecretsSharing.Controllers.Files.Dto.Response;

public class AllFileModelResponse
{
    [Required] 
    [JsonProperty("Files")] 
    public List<FileModelResponse> Files { get; init; }

    public AllFileModelResponse(List<FileModelResponse> files)
    {
        Files = files;
    }
}