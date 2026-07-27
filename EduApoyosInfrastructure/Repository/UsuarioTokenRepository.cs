using EduApoyosCommon.Interface;
using EduApoyosDomain.Entities;
using EduApoyosInfrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EduApoyosInfrastructure.Repository
{
    public class UsuarioTokenRepository : IUsuarioTokenRepository
    {
        private readonly AppDbContext _appDbContext;
        public UsuarioTokenRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task CrearTokenUsuarioAsync(UsuarioToken usuarioToken)
        {
            await _appDbContext.UsuarioTokens.AddAsync(usuarioToken);
        }
        public async Task DesactivarTokenUsuarioAsync(Guid usuarioId)
        {
            var tokens = await _appDbContext.UsuarioTokens.AsTracking().Where(t => t.UsuarioId == usuarioId && t.Activo).ToListAsync();
            foreach (var token in tokens)
            {
                token.Activo = false;
            }
        }
        public Task<UsuarioToken?> ObtenerUsuarioTokenByJwtIdAsync(string jwtId)
        {
            return _appDbContext.UsuarioTokens.FirstOrDefaultAsync(t => t.JwtId == jwtId);
        }
        public async Task GuardarCambiosAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
