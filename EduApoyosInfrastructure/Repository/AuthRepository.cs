using EduApoyosCommon.Interface;
using EduApoyosDomain.Dtos;
using EduApoyosDomain.Entities;
using EduApoyosInfrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EduApoyosInfrastructure.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _appContext;

        public AuthRepository(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task CrearUsuarioAsync(Usuario usuario)
        {
            await _appContext.Usuarios.AddAsync(usuario);
            await _appContext.SaveChangesAsync();
        }

        public Task<Usuario?> GetUsuarioByEmailAsync(string correoElectronico)
        {
            return _appContext.Usuarios.FirstOrDefaultAsync(
                    u => u.CorreoElectronico == correoElectronico);
        }
    }
}
