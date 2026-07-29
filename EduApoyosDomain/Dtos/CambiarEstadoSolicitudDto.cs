using EduApoyosDomain.Enums;

namespace EduApoyosDomain.Dtos
{
    public class CambiarEstadoSolicitudDto
    {
        public EstadoSolicitudEnum Estado { get; set; }
        public string Observacion { get; set; } = string.Empty;
    }
}
