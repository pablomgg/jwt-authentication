using Infrastructure.Application.Settings;
using Infrastructure.Identity.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Core
{
    public class Authentication : IAuthentication
    {
        private readonly AppSettings _appSettings;

        public Authentication(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings?.Value;
        }

        public async Task<string> GenerateToken(Guid aggregateId, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings?.TokenConfiguration?.Secret);
  
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role, role),
                    new Claim("Id", aggregateId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes((int)_appSettings?.TokenConfiguration?.Expiration).ToUniversalTime(),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return await Task.FromResult(tokenHandler.WriteToken(token));
        }

        public async Task<UserModel> DecodeToken(string accessToken = null)
        {
            if (accessToken is null)
                return await Task.FromResult(new UserModel());

            var handler = new JwtSecurityTokenHandler();
            var authHeader = accessToken.Replace("Bearer ", ""); 
            var token = handler.ReadToken(authHeader) as JwtSecurityToken;
            var id = token?.Claims?.FirstOrDefault(claim => claim.Type == "Id")?.Value;
            if(id is null)
                return await Task.FromResult(new UserModel());

            return await Task.FromResult(new UserModel() { Id = Guid.Parse(id) });
        }
    }
}