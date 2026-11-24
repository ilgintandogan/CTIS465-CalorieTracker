using CORE.Authentication.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CORE.Authentication.Services
{
    public class JwtAuthService : IJwtAuthService
    {
        public JwtResponse GetJwtResponse(int userId, string userName,
            IEnumerable<string> userRoleNames, DateTime expiration,
            string securityKey, string issuer,
            string audience, string refreshToken)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", userId.ToString()),
                new Claim(ClaimTypes.Name, userName)
            };
            foreach (var userRoleName in userRoleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRoleName));
            }

            var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(securityKey));
            var signingCredentials = new SigningCredentials(
                signingKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer, audience, claims, DateTime.Now, expiration,
                signingCredentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var jwt = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            return new JwtResponse
            {
                Jwt = $"{JwtBearerDefaults.AuthenticationScheme} {jwt}",
                RefreshToken = refreshToken
            };
        }

        public string GetRefreshToken()
        {
            var bytes = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes);
        }

        public IEnumerable<Claim> GetClaims(string jwt, string securityKey)
        {
            jwt = jwt.StartsWith(JwtBearerDefaults.AuthenticationScheme) ?
                jwt.Remove(0, JwtBearerDefaults.AuthenticationScheme.Length + 1) :
                jwt;

            var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(securityKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = jwtSecurityTokenHandler.ValidateToken(
                jwt, tokenValidationParameters, out securityToken);
            return securityToken is null ? null : principal.Claims;
        }
    }
}
