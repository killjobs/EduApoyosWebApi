using EduApoyosDomain.Enums;

namespace EduApoyosDomain.Dtos
{
    public class EstudianteDto
    {
        public Guid? Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string NumeroDocumento { get; set; } = string.Empty;
        public TipoDocumentoEnum TipoDocumento { get; set; }
        public ProgramaAcademicoEnum ProgramaAcademico { get; set; }
        public int Semestre { get; set; }
    }
}
