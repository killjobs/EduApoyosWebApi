using EduApoyosApplication.Implementation;
using EduApoyosCommon.Interface;
using EduApoyosDomain.Dtos;
using EduApoyosDomain.Entities;

namespace EduApoyosApplication
{
    public class AuthApplication : IAuthApplication
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUsuarioTokenRepository _usuarioTokenRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public AuthApplication(IAuthRepository authRepository, IUsuarioTokenRepository usuarioTokenRepository, IPasswordService passwordService, IJwtService jwtService)
        {
            _authRepository = authRepository;
            _usuarioTokenRepository = usuarioTokenRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        public async Task CrearUsuarioAsync(CrearUsuarioDto crearUsuarioDto)
        {
            var usuario = await _authRepository.GetUsuarioByEmailAsync(crearUsuarioDto.CorreoElectronico);

            if (usuario != null)
            {
                throw new UnauthorizedAccessException("El correo ya se encuentra registrado.");
            }

            string hashedPassword = _passwordService.HashPassword(crearUsuarioDto.Password);

            var nuevoUsuario = new Usuario
            {
                Id = Guid.NewGuid(),
                NombreCompleto = crearUsuarioDto.NombreCompleto,
                CorreoElectronico = crearUsuarioDto.CorreoElectronico,
                PasswordHash = hashedPassword,
                Rol = crearUsuarioDto.Rol,
                FechaRegistro = DateTime.UtcNow
            };

            await _authRepository.CrearUsuarioAsync(nuevoUsuario);
        }

        public async Task<ObjectResultDto<JwtResultDto>> LoginAsync(LoginUsuarioDto loginUsuarioDto)
        {
            var usuario = await _authRepository.GetUsuarioByEmailAsync(loginUsuarioDto.CorreoElectronico);

            if(usuario == null){
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");
            }

            bool isPasswordValid = _passwordService.VerifyPassword(usuario.PasswordHash, loginUsuarioDto.Password);

            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");
            }

            await _usuarioTokenRepository.DesactivarTokenUsuarioAsync(usuario.Id);

            var token = _jwtService.GenerarToken(usuario);

            var usuarioTokenDetail = new UsuarioToken
            {
                UsuarioId = usuario.Id,
                JwtId = token.JwtId,
                FechaCreacion = DateTime.UtcNow,
                FechaExpiracion = token.Expiration,
                Activo = true
            };

            await _usuarioTokenRepository.CrearTokenUsuarioAsync(usuarioTokenDetail);
            await _usuarioTokenRepository.GuardarCambiosAsync();

            return new ObjectResultDto<JwtResultDto>
            {
                Success = true,
                Data = token
            };
        }
    }
}
