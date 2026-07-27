using EduApoyosDomain.Entities;

namespace EduApoyosCommon.Interface
{
    public interface IUsuarioTokenRepository
    {
        Task DesactivarTokenUsuarioAsync(Guid usuarioId);
        Task CrearTokenUsuarioAsync(UsuarioToken usuarioToken);
        Task<UsuarioToken?> ObtenerUsuarioTokenByJwtIdAsync(string jwtId);
        Task GuardarCambiosAsync();
    }
}
