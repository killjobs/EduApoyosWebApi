using EduApoyosCommon.Interface;
using EduApoyosDomain.Entities;
using EduApoyosDomain.Enums;
using EduApoyosInfrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EduApoyosInfrastructure.Repository
{
    public class SolicitudRepository : ISolicitudRepository
    {
        private readonly AppDbContext _appDbContext;

        public SolicitudRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<int> CountAsync(EstadoSolicitudEnum? estado)
        {
            var query = _appDbContext.SolicitudesApoyo
                        .AsNoTracking()
                        .AsQueryable();

            if (estado.HasValue)
            {
                query = query.Where(x => x.Estado == estado.Value);
            }
            return await query.CountAsync();
        }
        public async Task CrearAsync(SolicitudApoyo solicitudApoyo)
        {
            await _appDbContext.SolicitudesApoyo.AddAsync(solicitudApoyo);
        }
        public async Task<List<SolicitudApoyo>> GetAsync(int page, int pageSize, EstadoSolicitudEnum? estado)
        {
            var query = _appDbContext.SolicitudesApoyo
                        .AsNoTracking()
                        .Include(x => x.Estudiante)
                        .AsQueryable();

            if (estado.HasValue)
            {
                query = query.Where(x => x.Estado == estado.Value);
            }

            return await query
                .OrderByDescending(x => x.FechaSolicitud)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<SolicitudApoyo?> GetByIdAsync(Guid solicitudId)
        {
            return await _appDbContext.SolicitudesApoyo
            .AsTracking()
            .Include(x => x.Estudiante)
            .Include(x => x.HistorialEstados)
            .FirstOrDefaultAsync(x => x.Id == solicitudId);
        }
        public async Task<SolicitudApoyo?> GetByIdHistorialAsync(Guid solicitudId)
        {
            var solicitud = await _appDbContext.SolicitudesApoyo
                .Include(x => x.Estudiante)
                .Include(x => x.Asesor)
                .Include(x => x.HistorialEstados)
                .FirstOrDefaultAsync(x => x.Id == solicitudId);

            if (solicitud is not null)
            {
                solicitud.HistorialEstados = solicitud.HistorialEstados
                    .OrderByDescending(x => x.FechaCambio)
                    .ToList();
            }

            return solicitud;
        }
        public async Task GuardarCambiosAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
        public async Task<bool> ExisteSolicitudActivaAsync(Guid estudianteId, TipoApoyoEnum tipoApoyo)
        {
            return await _appDbContext.SolicitudesApoyo.AnyAsync(x =>
                    x.EstudianteId == estudianteId && x.TipoApoyo == tipoApoyo &&
                    (x.Estado == EstadoSolicitudEnum.Pendiente ||x.Estado == EstadoSolicitudEnum.EnRevision));
        }
        public async Task AgregarHistorialAsync(HistorialEstado historial)
        {
            await _appDbContext.HistorialEstados.AddAsync(historial);
        }
    }
}
