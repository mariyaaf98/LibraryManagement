using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using LibraryManagement.Domain.UserEntity;
namespace LibraryManagement.Application.Common;
public class TokenService
{
    public string CreateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("UserId", user.Id.ToString())
        };

        var key = Environment.GetEnvironmentVariable("JWT_KEY");

        //to convert bytes → security key
        var securityKey = new SymmetricSecurityKey(
            // Converts string → binary key
            Encoding.UTF8.GetBytes(key!)
        );

        var creds = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );

        // token created
        var token = new JwtSecurityToken(

            //Who created the token
            issuer: Environment.GetEnvironmentVariable("JWT_ISSUER"),

            //Who is allowed to use the token
            audience: Environment.GetEnvironmentVariable("JWT_AUDIENCE"),

            //User data (email, role, id)
            claims: claims,

            expires: DateTime.Now.AddHours(2),

            // Security applied to token
            signingCredentials: creds
        );

                                            // convert token C# object → string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}