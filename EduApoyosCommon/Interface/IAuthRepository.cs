using EduApoyosDomain.Dtos;
using EduApoyosDomain.Entities;

namespace EduApoyosCommon.Interface
{
    public interface IAuthRepository
    {
        Task<Usuario?> GetUsuarioByEmailAsync(string correoElectronico);
        Task CrearUsuarioAsync(Usuario usuario);
    }
}
