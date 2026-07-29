using EduApoyosDomain.Enums;
using System.Text.Json.Serialization;

namespace EduApoyosDomain.Entities
{
    public class HistorialEstado
    {
        public Guid Id { get; set; }
        public Guid SolicitudId { get; set; }
        public EstadoSolicitudEnum EstadoAnterior { get; set; }
        public EstadoSolicitudEnum EstadoNuevo { get; set; }
        public DateTime FechaCambio { get; set; }
        public Guid UsuarioId { get; set; }
        public string Observacion { get; set; } = string.Empty;
        [JsonIgnore]
        public SolicitudApoyo Solicitud { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
    }
}
