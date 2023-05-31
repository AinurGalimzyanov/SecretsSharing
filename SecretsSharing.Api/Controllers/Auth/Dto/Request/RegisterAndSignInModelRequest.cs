using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SecretsSharing.Controllers.Auth.Dto.Request;

/// <summary>
/// input model with user register information
/// </summary>
public class RegisterAndSignInModelRequest
{
    /// <summary>
    /// user email
    /// </summary>
    [Required]
    [EmailAddress]
    [JsonProperty("Email")]
    public required string Email { get; init; }
    
    /// <summary>
    /// user password
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [JsonProperty("Password")]
    public required string Password { get; init; }
}