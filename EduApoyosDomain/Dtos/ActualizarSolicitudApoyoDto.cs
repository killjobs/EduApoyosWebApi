using EduApoyosDomain.Enums;

namespace EduApoyosDomain.Dtos
{
    public class ActualizarSolicitudApoyoDto
    {
        public Guid Id { get; set; }
        public TipoApoyoEnum TipoApoyo { get; set; }
        public decimal MontoSolicitado { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}
