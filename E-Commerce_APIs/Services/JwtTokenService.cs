using ECommerce.APIs;
using ECommerce.DAL;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.API.Services;

public class JwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> options)
    {
        _jwtSettings = options.Value;
    }

    public TokenDto CreateToken(AppUser user, IEnumerable<string> roles)
    {
        //Dynamic claims based on user info and roles (Feature Workflow)
        var claims = BuildClaims(user, roles);

        var key = new SymmetricSecurityKey(
            Convert.FromBase64String(_jwtSettings.SecretKey)
       
        );

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);


        var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            signingCredentials: signingCredentials,
            expires: expiration
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        
        return new TokenDto(token, expiration);
    }

    private List<Claim> BuildClaims(AppUser user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email ?? ""),
        new Claim(ClaimTypes.Name, user.UserName ?? "")
    };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        return claims;
    }
}