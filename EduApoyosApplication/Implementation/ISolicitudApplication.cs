using EduApoyosDomain.Dtos;
using EduApoyosDomain.Entities;
using EduApoyosDomain.Enums;

namespace EduApoyosApplication.Implementation
{
    public interface ISolicitudApplication
    {
        Task CrearAsync(CrearSolicitudApoyoDto solicitudDto);
        Task<ObjectResultDto<PagedResultDto<SolicitudApoyo>>>GetSolicitudesAsync(int page,int pageSize);
        Task<ObjectResultDto<SolicitudApoyo>> GetByIdHistorialAsync(Guid solicitudId, Guid usuarioId, RolUsuario rol);
        Task CambiarEstadoAsync(Guid id,CambiarEstadoSolicitudDto solicitudDto, Guid usuarioId);
        Task ActualizarAsync(ActualizarSolicitudApoyoDto solicitudDto);
    }
}
