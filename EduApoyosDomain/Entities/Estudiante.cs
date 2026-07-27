namespace EduApoyosDomain.Entities
{
    public class Estudiante
    {
        public Guid Id { get; set; }

        public Guid UsuarioId { get; set; }

        public string NumeroDocumento { get; set; } = string.Empty;

        public string TipoDocumento { get; set; } = string.Empty;

        public string ProgramaAcademico { get; set; } = string.Empty;

        public int Semestre { get; set; }

        public Usuario Usuario { get; set; } = null!;
    }
}
