using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Vendor.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Vendor.Infrastructure
{
    public class PrivilegeAssist
    {
        private IConfiguration Configuration;

        public PrivilegeAssist(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public AccessTokenModel GenerateToken(Staff staff)
        {
            AccessTokenModel token = new AccessTokenModel();
            token.AccessToken = GenerateAccessToken(staff);
            token.RefreshToken = GenerateRefreshToken();
            return token;
        }

        public ClaimsPrincipal? GetTokenPrincipal(string token)
        {
            var validation = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                RequireExpirationTime = false
            };
            return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }

        private string GenerateAccessToken(Staff staff)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(staff),
                Expires = DateTime.Now.AddSeconds(Convert.ToInt32(Configuration["Jwt:Expires"])),
                Issuer = Configuration["Jwt:Issuer"],
                Audience = Configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using (var numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }

        private static ClaimsIdentity GenerateClaims(Staff staff)
        {
            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim("Id", staff.Id.ToString()));
            //claims.AddClaim(new Claim("Email", staff.Email.ToString()));
            //claims.AddClaim(new Claim("Phone", staff.Phone));

            return claims;
        }

    }
}
