using IdServer.Core.QueryModel;
using IdServer.Core.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdServer.Infraestructure.Services
{
    public class SecurityTokenProvider : ISecurityTokenProvider
    {
        private readonly ILogger _logger;
        private readonly SecurityOptions _options;

        public SecurityTokenProvider(ILogger<SecurityTokenProvider> logger, IOptions<SecurityOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public string GenerateAccessToken(UserIdentity userIdentity)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.AccessTokenKeySecret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, userIdentity.UserName),
                        new Claim(JwtRegisteredClaimNames.Email, userIdentity.Email),
                        new Claim(ClaimTypes.PrimarySid, userIdentity.UserId.ToString()),
                        new Claim(JwtRegisteredClaimNames.Name, $"{userIdentity.FirstName} {userIdentity.LastName}"),
                        new Claim(nameof(userIdentity.ClubCode), userIdentity.ClubCode),
                        new Claim(ClaimTypes.Role, userIdentity.RoleCode)
                    }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = _options.Issuer,
                Audience = userIdentity.Scope,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateToken(int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecurityTokenKeySecret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    }),
                Expires = DateTime.UtcNow.AddMinutes(2),
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string token, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecurityTokenKeySecret));
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var claims = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _options.Issuer,
                    ValidAudience = _options.Audience,
                    IssuerSigningKey = securityKey
                }, out SecurityToken validatedToken);
                return claims.FindFirstValue(ClaimTypes.NameIdentifier) == userId.ToString();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
