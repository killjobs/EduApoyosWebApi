using EduApoyosApplication.Implementation;
using EduApoyosCommon.Interface;
using EduApoyosDomain.Dtos;
using EduApoyosDomain.Entities;
using EduApoyosDomain.Enums;

namespace EduApoyosApplication
{
    public class SolicitudApplication : ISolicitudApplication
    {
        private readonly ISolicitudRepository _solicitudRepository;

        public SolicitudApplication(ISolicitudRepository solicitudRepository)
        {
            _solicitudRepository = solicitudRepository;
        }
        public async Task<ObjectResultDto<PagedResultDto<SolicitudApoyo>>> GetSolicitudesAsync(int page, int pageSize, EstadoSolicitudEnum? estado)
        {
            var solicitudes = await _solicitudRepository.GetAsync(page, pageSize, estado);
            var totalRecords = await _solicitudRepository.CountAsync(estado);

            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            return new ObjectResultDto<PagedResultDto<SolicitudApoyo>>
            {
                Success = true,
                Data = new PagedResultDto<SolicitudApoyo>
                {
                    Items = solicitudes,
                    Page = page,
                    PageSize = pageSize,
                    TotalRecords = totalRecords,
                    TotalPages = totalPages
                }
            };
        }
        public async Task<ObjectResultDto<SolicitudApoyo>> GetByIdHistorialAsync(Guid solicitudId, Guid usuarioId, RolUsuario rol)
        {
            var solicitud = await _solicitudRepository.GetByIdAsync(solicitudId);
            if (rol == RolUsuario.Estudiante && solicitud.Estudiante.UsuarioId != usuarioId)
            {
                throw new UnauthorizedAccessException("No tiene permisos para consultar esta solicitud.");
            }

            return new ObjectResultDto<SolicitudApoyo>
            {
                Data = await _solicitudRepository.GetByIdHistorialAsync(solicitudId),
                Success = true
            };
        }
        public async Task CrearAsync(CrearSolicitudApoyoDto solicitudDto)
        {
            if (await _solicitudRepository.ExisteSolicitudActivaAsync(solicitudDto.EstudianteId, solicitudDto.TipoApoyo))
            {
                throw new InvalidOperationException("El estudiante ya tiene una solicitud en proceso para el tipo de apoyo.");
            }
            var solicitud = new SolicitudApoyo
            {
                Id = Guid.NewGuid(),
                EstudianteId = solicitudDto.EstudianteId,
                TipoApoyo = solicitudDto.TipoApoyo,
                MontoSolicitado = solicitudDto.MontoSolicitado,
                Descripcion = solicitudDto.Descripcion,
                Estado = EstadoSolicitudEnum.Pendiente,
                FechaSolicitud = DateTime.UtcNow,
                FechaActualizacion = DateTime.UtcNow,
                AsesorId = solicitudDto.AsesorId
            };

            solicitud.HistorialEstados.Add(
                new HistorialEstado
                {
                    Id = Guid.NewGuid(),
                    SolicitudId = solicitud.Id,
                    EstadoAnterior = EstadoSolicitudEnum.Pendiente,
                    EstadoNuevo = EstadoSolicitudEnum.Pendiente,
                    FechaCambio = DateTime.UtcNow,
                    UsuarioId = solicitudDto.AsesorId,
                    Observacion = "Solicitud creada"
                });

            await _solicitudRepository.CrearAsync(solicitud);
            await _solicitudRepository.GuardarCambiosAsync();
        }
        public async Task CambiarEstadoAsync(Guid id,CambiarEstadoSolicitudDto solicitudDto, Guid usuarioId)
        {
            var solicitud = await _solicitudRepository.GetByIdAsync(id);

            if (solicitud == null)
            {
                throw new InvalidOperationException("Solicitud no encontrada.");
            }

            var estadoAnterior = solicitud.Estado;

            if (estadoAnterior == solicitudDto.Estado)
            {
                throw new InvalidOperationException("La solicitud ya se encuentra en ese estado.");
            }

            solicitud.Estado = solicitudDto.Estado;
            solicitud.FechaActualizacion =DateTime.UtcNow;
            var historial = new HistorialEstado
            {
                Id = Guid.NewGuid(),
                SolicitudId = solicitud.Id,
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = solicitudDto.Estado,
                FechaCambio = DateTime.UtcNow,
                UsuarioId = usuarioId,
                Observacion = solicitudDto.Observacion
            };

            await _solicitudRepository.AgregarHistorialAsync(historial);
            await _solicitudRepository.GuardarCambiosAsync();
        }
        public async Task ActualizarAsync(ActualizarSolicitudApoyoDto solicitudDto)
        {
            var solicitud = await _solicitudRepository.GetByIdAsync(solicitudDto.Id);

            if (solicitud == null)
            {
                throw new InvalidOperationException($"La solicitud {solicitudDto.Id} no existe.");
            }

            solicitud.TipoApoyo = solicitudDto.TipoApoyo;
            solicitud.MontoSolicitado = solicitudDto.MontoSolicitado;
            solicitud.Descripcion = solicitudDto.Descripcion;
            solicitud.FechaActualizacion = DateTime.UtcNow;

            await _solicitudRepository.GuardarCambiosAsync();
        }

    }
}
