using Microsoft.Extensions.Configuration;
using RegistrationApp.BusinessLogic.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RegistrationApp.BusinessLogic.Services
{
    public class JwtService : IJwtService
    {
        // Configuration object to read settings from appsettings.json
        private readonly IConfiguration _configuration;

        // Constructor accepts IConfiguration object to initialize _configuration field
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetJwtToken(string userId, string role)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role)
            };

            // Retrieve  secret key for signing the token from the configuration
            var secretToken = _configuration.GetSection("Jwt:Key").Value;

            // Convert secret key to a byte array and create a SymmetricSecurityKey
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretToken));

            // Create signing credentials using the symmetric key and HMAC-SHA512 algorithm
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("Jwt:Issuer").Value, // Token issuer
                audience: _configuration.GetSection("Jwt:Audience").Value, // Token audience
                claims: claims, // Claims to be included in the token
                expires: DateTime.Now.AddMinutes(20), // Token expiration time
                signingCredentials: cred); // Signing credentials

            // Serialize the token to a string and return it
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
