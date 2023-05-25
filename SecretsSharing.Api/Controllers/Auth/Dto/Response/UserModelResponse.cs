using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SecretsSharing.Controllers.Auth.Dto.Response;

public class UserModelResponse
{
    [Required] 
    [JsonProperty("AccessToken")] 
    public string AccessToken { get; init; }
    
    [Required] 
    [JsonProperty("Email")] 
    public string Email { get; init; }

    public UserModelResponse(string accessToken, string email)
    {
        AccessToken = accessToken;
        Email = email;
    }
}