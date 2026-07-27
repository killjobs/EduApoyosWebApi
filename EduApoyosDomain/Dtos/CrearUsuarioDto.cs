
using EduApoyosDomain.Enums;

namespace EduApoyosDomain.Dtos
{
    public class CrearUsuarioDto
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public RolUsuario Rol { get; set; }
    }
}
