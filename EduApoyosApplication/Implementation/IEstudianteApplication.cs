using EduApoyosDomain.Dtos;
using EduApoyosDomain.Entities;

namespace EduApoyosApplication.Implementation
{
    public interface IEstudianteApplication
    {
        Task<ObjectResultDto<List<Estudiante>>> GetEstudiantesAsync();
    }
}
