using EduApoyosDomain.Entities;
using EduApoyosDomain.Enums;

namespace EduApoyosCommon.Interface
{
    public interface ISolicitudRepository
    {
        Task CrearAsync(SolicitudApoyo solicitudApoyo);
        Task<List<SolicitudApoyo>> GetAsync(int page, int pageSize, EstadoSolicitudEnum? estado);
        Task<SolicitudApoyo?> GetByIdAsync(Guid solicitudId);
        Task<SolicitudApoyo?> GetByIdHistorialAsync(Guid solicitudId);
        Task<int> CountAsync(EstadoSolicitudEnum? estado);
        Task GuardarCambiosAsync();
        Task<bool> ExisteSolicitudActivaAsync(Guid estudianteId, TipoApoyoEnum tipoApoyo);
        Task AgregarHistorialAsync(HistorialEstado historial);
    }
}
