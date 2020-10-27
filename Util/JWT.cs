using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace RecYouBackend.Util
{
    public class JWT
    {
        public static string GenerateToken(string username)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSecrets.json", optional: false, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetConnectionString("StreamAPISecret")));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {           
                    new Claim(ClaimTypes.NameIdentifier, username),                
                }),
                Expires = DateTime.UtcNow.AddYears(10),
                Issuer = configuration.GetConnectionString("JwtIssuer"),
                Audience = configuration.GetConnectionString("JwtAud"),
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
