using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api;
using AutoMapper;
using Dal.User.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SecretsSharing.Controllers.Auth.Dto.Request;
using SecretsSharing.Controllers.Auth.Dto.Response;
using SecretsSharing.Controllers.Base;

namespace SecretsSharing.Controllers.Auth;

/// <summary>
/// controller that works with requests related to user authorization
/// </summary>
public class AuthorizeController : BaseController
{
    private readonly SignInManager<UserDal> _signInManager;
    private readonly UserManager<UserDal> _userManager;
    private readonly JWTSettings _options;
    private readonly IMapper _mapper;

    /// <summary>
    /// controller constructor
    /// </summary>
    /// <param name="userManager">User logic service</param>
    /// <param name="signInManager">Logic service for working with user authentication</param>
    /// <param name="options">Unique token settings</param>
    /// <param name="mapper">automapper</param>
    public AuthorizeController(UserManager<UserDal> userManager, 
        SignInManager<UserDal> signInManager, 
        IOptions<JWTSettings> options,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _options = options.Value;
        _mapper = mapper;
    }
    
    /// <summary>
    /// registers a new user
    /// </summary>
    /// <param name="model">the model of information about the registered user</param>
    /// <returns>information about the user or BadRequest</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterAndSignInModelRequest model)
    {
        var user = _mapper.Map<UserDal>(model);
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Email, user.Email)
            };
            await _userManager.AddClaimsAsync(user, claims);
            var accessToken = GetToken(claims, 15);
            var refreshToken = GetToken(claims, 43200);
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);
            HttpContext.Response.Cookies.Append(".AspNetCore.Application.RefreshToken", refreshToken);
            return Ok(new UserModelResponse("Bearer " + accessToken, user.Email));
        }

        return BadRequest();
    }
    
    /// <summary>
    /// route for creating a new token(access or refresh)
    /// </summary>
    /// <param name="principal">claims for token</param>
    /// <param name="timeMin">token lifetime in min</param>
    /// <returns>token</returns>
    private string GetToken(IEnumerable<Claim> principal, int timeMin)
    {
        var claims = principal.ToList();
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretKey));
        var token = new JwtSecurityToken
        (
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(timeMin),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return tokenHandler.WriteToken(token);
    }
    
    /// <summary>
    /// route for finding a user by token
    /// </summary>
    /// <param name="token">user token</param>
    /// <returns>UserDal</returns>
    private async Task<UserDal> FindUserByToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var email = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        return await _userManager.FindByEmailAsync(email);;
    }
    
    /// <summary>
    /// the route that logs the user
    /// </summary>
    /// <param name="model">the model of information about the signin user</param>
    /// <returns>information about the user or Unauthorized</returns>
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] RegisterAndSignInModelRequest model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, false, false);
        
        if (result.Succeeded) 
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var accessToken = GetToken(claims, 15);
            var refreshToken = HttpContext.Request.Cookies[".AspNetCore.Application.RefreshToken"];
            if (refreshToken == null)
            { 
                HttpContext.Response.Cookies.Append(".AspNetCore.Application.RefreshToken", user.RefreshToken);
            }
            return Ok(new UserModelResponse("Bearer " + accessToken,  user.Email));
        }

        return Unauthorized();
    }
    
    /// <summary>
    /// the route that updates refresh token
    /// </summary>
    /// <returns>information about the user or BadRequest</returns>
    [HttpPatch("refreshAccessToken")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = HttpContext.Request.Cookies[".AspNetCore.Application.RefreshToken"];
        var user = await FindUserByToken(refreshToken);
        if(user != null || user.RefreshToken == refreshToken)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var newAccessToken = GetToken(claims, 15);
            var newRefreshToken = GetToken(claims, 43200);
            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);
            HttpContext.Response.Cookies.Append(".AspNetCore.Application.RefreshToken", newRefreshToken);
            return Ok(new UserModelResponse("Bearer " + newAccessToken, user.Email));
        }
        
        return BadRequest();
    }
}