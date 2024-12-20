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

        public AccessTokenModel GenerateToken(User user)
        {
            AccessTokenModel token = new AccessTokenModel();
            token.AccessToken = GenerateAccessToken(user);
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

        private string GenerateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
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

        private static ClaimsIdentity GenerateClaims(User user)
        {
            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim("Id", user.Id.ToString()));
            //claims.AddClaim(new Claim("Email", user.Email.ToString()));
            //claims.AddClaim(new Claim("Phone", user.Phone));

            return claims;
        }

    }
}
