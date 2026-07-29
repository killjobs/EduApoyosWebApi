using EduApoyosDomain.Entities;
using EduApoyosDomain.Enums;

namespace EduApoyosCommon.Interface
{
    public interface ISolicitudRepository
    {
        Task CrearAsync(SolicitudApoyo solicitudApoyo);
        Task<List<SolicitudApoyo>> GetAsync(int page, int pageSize);
        Task<SolicitudApoyo?> GetByIdAsync(Guid solicitudId);
        Task<SolicitudApoyo?> GetByIdHistorialAsync(Guid solicitudId);
        Task<int> CountAsync();
        Task GuardarCambiosAsync();
        Task<bool> ExisteSolicitudActivaAsync(Guid estudianteId, TipoApoyoEnum tipoApoyo);
        Task AgregarHistorialAsync(HistorialEstado historial);
    }
}
