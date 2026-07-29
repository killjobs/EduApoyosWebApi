using EduApoyosDomain.Enums;

namespace EduApoyosDomain.Entities
{
    public class SolicitudApoyo
    {
        public Guid Id { get; set; }
        public Guid EstudianteId { get; set; }
        public TipoApoyoEnum TipoApoyo { get; set; }
        public decimal MontoSolicitado { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public EstadoSolicitudEnum Estado { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public Guid AsesorId { get; set; }
        public Estudiante Estudiante { get; set; } = null!;
        public Usuario Asesor { get; set; } = null!;
        public ICollection<HistorialEstado> HistorialEstados { get; set; } = new List<HistorialEstado>();
    }
}
