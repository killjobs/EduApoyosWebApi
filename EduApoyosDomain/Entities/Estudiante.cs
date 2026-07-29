using EduApoyosDomain.Enums;
using System.Text.Json.Serialization;

namespace EduApoyosDomain.Entities
{
    public class Estudiante
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string NumeroDocumento { get; set; } = string.Empty;
        public TipoDocumentoEnum TipoDocumento { get; set; }
        public ProgramaAcademicoEnum ProgramaAcademico { get; set; }
        public int Semestre { get; set; }
        public Usuario Usuario { get; set; } = null!;
        [JsonIgnore]
        public ICollection<SolicitudApoyo> Solicitudes { get; set; } = new List<SolicitudApoyo>();
    }
}
