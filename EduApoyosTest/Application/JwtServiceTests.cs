using System.IdentityModel.Tokens.Jwt;
using EduApoyosApplication;
using EduApoyosApplication.Settings;
using EduApoyosDomain.Entities;
using EduApoyosDomain.Enums;
using Microsoft.Extensions.Options;

namespace EduApoyosTest.Application;

public class JwtServiceTests
{
    private readonly JwtService _service;

    public JwtServiceTests()
    {
        var settings = Options.Create(
            new JwtSettings{
                SecretKey = "THIS_IS_A_SECRET_KEY_WITH_MORE_THAN_32_CHARS",
                Issuer = "EduApoyos",
                Audience = "EduApoyosUsers",
                ExpirationMinutes = 60
            });

        _service = new JwtService(settings);
    }

    [Fact]
    public void GenerarToken_Should_ReturnValidJwtResult()
    {
        var usuario = new Usuario {
            Id = Guid.NewGuid(),
            CorreoElectronico = "test@test.com",
            Rol = RolUsuario.Asesor
        };

        var result = _service.GenerarToken(usuario);

        Assert.NotNull(result);
        Assert.NotNull(result.Token);
        Assert.NotNull(result.JwtId);
        Assert.True(result.Expiration > DateTime.UtcNow);
    }

    [Fact]
    public void GenerarToken_Should_ContainExpectedClaims()
    {
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            CorreoElectronico = "test@test.com",
            Rol = RolUsuario.Asesor
        };

        var result = _service.GenerarToken(usuario);
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(result.Token);

        Assert.Contains(jwt.Claims,x => x.Type == JwtRegisteredClaimNames.Sub);
        Assert.Contains(jwt.Claims,x => x.Type == JwtRegisteredClaimNames.Email);
        Assert.Contains(jwt.Claims,x => x.Type == "role");
        Assert.Contains(jwt.Claims,x => x.Type == JwtRegisteredClaimNames.Jti);
    }
}