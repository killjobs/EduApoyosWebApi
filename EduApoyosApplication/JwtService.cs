using EduApoyosApplication.Implementation;
using EduApoyosApplication.Settings;
using EduApoyosDomain.Dtos;
using EduApoyosDomain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EduApoyosApplication
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public JwtResultDto GenerarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var jwtId = Guid.NewGuid().ToString();
            var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.CorreoElectronico),
                new Claim(ClaimTypes.Role, usuario.Rol.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, jwtId)
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new JwtResultDto
            {
                Token = tokenHandler.WriteToken(token),
                JwtId = jwtId,
                Expiration = expiration
            };
        }
    }
}
