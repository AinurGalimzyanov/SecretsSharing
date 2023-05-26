namespace Api;

public class JWTSettings
{
    public string SecretKey { get; init; } = "this is my custom Secret key for authentication";
    
    public string Issuer { get; init; } = "false";
    
    public string Audience { get; init; } = "sdff";
}