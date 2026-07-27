using EduApoyosDomain.Entities;

namespace EduApoyosCommon.Interface
{
    public interface IEstudianteRepository
    {
        Task<List<Estudiante>> GetEstudiantesAsync();
    }
}
