using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PersonalTasksProject.Configuration;
using PersonalTasksProject.Entities;
using PersonalTasksProject.Providers.Interfaces;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PersonalTasksProject.Providers;

public sealed class TokenProvider : ITokenProvider
{
    private readonly JwtSecurityTokenHandler _handler = new();
    private readonly IAppConfiguration _configuration;
    
    public TokenProvider(IAppConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string CreateToken(User user)
    {   
        var credentials = _buildCredentials();

        var tokenDescriptor = _buildTokenDescriptor(credentials, user);
            
        var token = _handler.CreateToken(tokenDescriptor);
        
        return _handler.WriteToken(token);
    }
    
    public string DecodeToken(string token)
    {
        var splittedToken = token.Split(" ").First(c => c != "Bearer");

        var decodedToken = _handler.ReadToken(splittedToken);

        var tokenInfo = decodedToken as JwtSecurityToken;
        return tokenInfo.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Email).Value;

    }

    private SigningCredentials _buildCredentials()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Jwt.Secret));
        
        return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    }

    private SecurityTokenDescriptor _buildTokenDescriptor(SigningCredentials credentials, User user)
    {
        var now = DateTime.UtcNow;

        return new SecurityTokenDescriptor
                       {
                           Subject = new ClaimsIdentity(
                           [
                               new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                               new Claim(JwtRegisteredClaimNames.Email, user.Email),
                           ]),
                           NotBefore = now.AddSeconds(-30),
                           Expires = now.AddMinutes(_configuration.Jwt.ExpireMinutes),
                           SigningCredentials = credentials,
                           Issuer = _configuration.Jwt.Issuer,
                           Audience = _configuration.Jwt.Audience,
                       };
    }
}