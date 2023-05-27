using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Files.Dto.Request;

public class TextModelRequest
{
    [Required]
    [DefaultValue("")]
    [JsonProperty("Text")]
    public required string? Text { get; init; }
    
    [Required]
    [JsonProperty("Name")]
    public required string? Name { get; init; }

    [Required]
    [DefaultValue(false)]
    [JsonProperty("Cascade")] 
    public required bool Cascade { get; init; } 
}