namespace EduApoyosDomain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int Rol { get; set; }
        public DateTime FechaRegistro { get; set; }
        public Estudiante? Estudiante { get; set; }
        public ICollection<UsuarioToken> Tokens { get; set; } = new List<UsuarioToken>();
    }
}
