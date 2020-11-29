using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace RecYouBackend.Util
{
    /* 
     * Utility class to generate new JWT
     * Uses the StreamAPISecret as the security key
     * Sets the Issuer and the audience
     * Information obtained in appSecrets.json
    */
    public class JWT
    {
        public static string GenerateToken(string username)
        {
            // Get access to the appsecrets.json file
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSecrets.json", optional: false, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            // Get secret to sign token
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetConnectionString("StreamAPISecret")));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Set an username claim inside the JWT
                Subject = new ClaimsIdentity(new Claim[]
                {           
                    new Claim(ClaimTypes.Name, username),                
                }),
                // Expiration in 10 years
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
