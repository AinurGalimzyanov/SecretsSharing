using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SecretsSharing.Controllers.Files.Dto.Request;

/// <summary>
/// request model with information about the uploaded text
/// </summary>
public class TextModelRequest
{
    /// <summary>
    /// uploadable text 
    /// </summary>
    [Required]
    [DefaultValue("")]
    [JsonProperty("Text")]
    public required string? Text { get; init; }
    
    /// <summary>
    /// file name
    /// </summary>
    [Required]
    [JsonProperty("Name")]
    public required string? Name { get; init; }

    /// <summary>
    /// a value that determines whether to delete a file after accessing it
    /// </summary>
    [Required]
    [DefaultValue(false)]
    [JsonProperty("Cascade")] 
    public required bool Cascade { get; init; } 
}