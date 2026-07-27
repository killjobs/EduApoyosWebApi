using EduApoyosDomain.Dtos;

namespace EduApoyosApplication.Implementation
{
    public interface IAuthApplication
    {
        Task<ObjectResultDto<JwtResultDto>> LoginAsync(LoginUsuarioDto loginUsuarioDto);
        Task CrearUsuarioAsync(CrearUsuarioDto crearUsuarioDto);
    }
}
