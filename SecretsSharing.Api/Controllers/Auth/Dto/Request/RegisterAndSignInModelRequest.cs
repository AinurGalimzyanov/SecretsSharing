using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SecretsSharing.Controllers.Auth.Dto.Request;

public class RegisterAndSignInModelRequest
{
    [Required]
    [EmailAddress]
    [JsonProperty("Email")]
    public required string Email { get; init; }
    
    [Required]
    [DataType(DataType.Password)]
    [JsonProperty("Password")]
    public required string Password { get; init; }
}