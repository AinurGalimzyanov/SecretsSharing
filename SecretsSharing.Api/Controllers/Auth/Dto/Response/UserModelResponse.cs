using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SecretsSharing.Controllers.Auth.Dto.Response;

/// <summary>
/// response model with user data
/// </summary>
public class UserModelResponse
{
    /// <summary>
    /// user access token
    /// </summary>
    [Required] 
    [JsonProperty("AccessToken")] 
    public string AccessToken { get; init; }
    
    /// <summary>
    /// user email
    /// </summary>
    [Required] 
    [JsonProperty("Email")] 
    public string Email { get; init; }

    public UserModelResponse(string accessToken, string email)
    {
        AccessToken = accessToken;
        Email = email;
    }
}