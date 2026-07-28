using EduApoyosDomain.Entities;

namespace EduApoyosCommon.Interface
{
    public interface IEstudianteRepository
    {
        Task<List<Estudiante>> GetEstudiantesAsync(int page, int pageSize);
        Task<int> CountEstudiantesAsync();
        Task<Estudiante?> GetEstudianteByIdAsync(Guid id);
        Task<Estudiante?> GetEstudianteByNumeroDocumentoAsync(string numeroDocumento);
        Task CrearAsync(Estudiante estudiante);
        Task EliminarAsync(Estudiante estudiante);
        Task GuardarCambiosAsync();
    }
}
