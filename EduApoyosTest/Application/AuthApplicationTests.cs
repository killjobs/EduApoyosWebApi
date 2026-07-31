using EduApoyosApplication;
using EduApoyosApplication.Implementation;
using EduApoyosCommon.Interface;
using EduApoyosDomain.Dtos;
using EduApoyosDomain.Entities;
using EduApoyosDomain.Enums;
using Moq;

namespace EduApoyosTest.Application;

public class AuthApplicationTests
{
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IUsuarioTokenRepository> _usuarioTokenRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<IJwtService> _jwtServiceMock;

    private readonly AuthApplication _application;
    public AuthApplicationTests()
    {
        _authRepositoryMock = new Mock<IAuthRepository>();
        _usuarioTokenRepositoryMock = new Mock<IUsuarioTokenRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _jwtServiceMock = new Mock<IJwtService>();
        _application = new AuthApplication(_authRepositoryMock.Object,_usuarioTokenRepositoryMock.Object,_passwordServiceMock.Object,_jwtServiceMock.Object);
    }
    [Fact]
    public async Task CrearUsuarioAsync_Should_ThrowUnauthorizedAccessException_When_EmailAlreadyExists()
    {
        var dto = new CrearUsuarioDto
        {
            CorreoElectronico = "test@test.com"
        };
        _authRepositoryMock.Setup(x => x.GetUsuarioByEmailAsync(dto.CorreoElectronico)).ReturnsAsync(new Usuario());

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _application.CrearUsuarioAsync(dto));
    }
    [Fact]
    public async Task CrearUsuarioAsync_Should_CreateUser_When_EmailDoesNotExist()
    {
        var usuarioDto = new CrearUsuarioDto{
            NombreCompleto = "Juan Perez",
            CorreoElectronico = "test@test.com",
            Password = "Password123",
            Rol = RolUsuario.Asesor
        };
        _authRepositoryMock.Setup(x => x.GetUsuarioByEmailAsync(usuarioDto.CorreoElectronico)).ReturnsAsync((Usuario?)null);
        _passwordServiceMock.Setup(x => x.HashPassword(usuarioDto.Password)).Returns("HASH");

        await _application.CrearUsuarioAsync(usuarioDto);

        _passwordServiceMock.Verify(
            x => x.HashPassword(usuarioDto.Password),
            Times.Once
        );

        _authRepositoryMock.Verify(x => x.CrearUsuarioAsync(
            It.Is<Usuario>(u =>u.CorreoElectronico == usuarioDto.CorreoElectronico && u.NombreCompleto == usuarioDto.NombreCompleto && u.PasswordHash == "HASH")),
            Times.Once);
    }

    [Fact]
    public async Task LoginAsync_Should_ThrowUnauthorizedAccessException_When_UserDoesNotExist()
    {
        var dto = new LoginUsuarioDto
        {
            CorreoElectronico = "test@test.com",
            Password = "123"
        };
        _authRepositoryMock.Setup(x => x.GetUsuarioByEmailAsync(dto.CorreoElectronico)).ReturnsAsync((Usuario?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _application.LoginAsync(dto));
    }

    [Fact]
    public async Task LoginAsync_Should_ThrowUnauthorizedAccessException_When_PasswordIsInvalid()
    {
        var usuario = new Usuario
        {
            CorreoElectronico = "test@test.com",
            PasswordHash = "HASH"
        };
        var loginDto = new LoginUsuarioDto
        {
            CorreoElectronico = "test@test.com",
            Password = "123"
        };
        _authRepositoryMock.Setup(x => x.GetUsuarioByEmailAsync(loginDto.CorreoElectronico)).ReturnsAsync(usuario);
        _passwordServiceMock.Setup(x => x.VerifyPassword(usuario.PasswordHash, loginDto.Password)).Returns(false);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _application.LoginAsync(loginDto));
    }

    [Fact]
    public async Task LoginAsync_Should_ReturnToken_When_CredentialsAreValid()
    {
        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            CorreoElectronico = "test@test.com",
            PasswordHash = "HASH",
            Rol = RolUsuario.Asesor
        };
        var dto = new LoginUsuarioDto
        {
            CorreoElectronico = usuario.CorreoElectronico,
            Password = "Password123"
        };
        var token = new JwtResultDto
        {
            Token = "TOKEN",
            JwtId = Guid.NewGuid().ToString(),
            Expiration = DateTime.UtcNow.AddHours(1)
        };
        _authRepositoryMock.Setup(x => x.GetUsuarioByEmailAsync(dto.CorreoElectronico)).ReturnsAsync(usuario);
        _passwordServiceMock.Setup(x => x.VerifyPassword(usuario.PasswordHash, dto.Password)).Returns(true);
        _jwtServiceMock.Setup(x => x.GenerarToken(usuario)).Returns(token);

        var result = await _application.LoginAsync(dto);

        Assert.True(result.Success);
        Assert.Equal(token.Token, result.Data.Token);
        _usuarioTokenRepositoryMock.Verify(
            x => x.DesactivarTokenUsuarioAsync(usuario.Id),
            Times.Once
        );
        _usuarioTokenRepositoryMock.Verify(
            x => x.CrearTokenUsuarioAsync(It.IsAny<UsuarioToken>()),
            Times.Once
        );
        _usuarioTokenRepositoryMock.Verify(
            x => x.GuardarCambiosAsync(),
            Times.Once
        );
    }
}