using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SecretsSharing.Controllers.Files.Dto.Response;

public class FileModelResponse
{
    [Required] 
    [JsonProperty("Name")] 
    public string Name { get; init; }

    public FileModelResponse(string name)
    {
        Name = name;
    }
}