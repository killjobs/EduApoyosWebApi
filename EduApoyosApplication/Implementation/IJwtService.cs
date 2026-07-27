using EduApoyosDomain.Dtos;
using EduApoyosDomain.Entities;

namespace EduApoyosApplication.Implementation
{
    public interface IJwtService
    {
        JwtResultDto GenerarToken(Usuario usuario);
    }
}
