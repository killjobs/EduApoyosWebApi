using EduApoyosDomain.Enums;

namespace EduApoyosDomain.Dtos
{
    public class CrearSolicitudApoyoDto
    {
        public Guid EstudianteId { get; set; }
        public TipoApoyoEnum TipoApoyo { get; set; }
        public decimal MontoSolicitado { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public Guid AsesorId { get; set; }
    }
}
