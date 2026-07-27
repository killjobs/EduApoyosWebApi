namespace EduApoyosDomain.Entities
{
    public class UsuarioToken
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string JwtId { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool Activo { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}
