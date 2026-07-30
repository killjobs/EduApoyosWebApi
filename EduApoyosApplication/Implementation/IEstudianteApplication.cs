using EduApoyosDomain.Dtos;
using EduApoyosDomain.Entities;

namespace EduApoyosApplication.Implementation
{
    public interface IEstudianteApplication
    {
        Task<ObjectResultDto<PagedResultDto<Estudiante>>> GetEstudiantesAsync(int page, int pageSize);
        Task<ObjectResultDto<Estudiante>> GetEstudianteByIdAsync(Guid id);
        Task<ObjectResultDto<Estudiante>> GetEstudianteByNumeroDocumentoAsync(string numeroDocumento);
        Task CrearAsync(EstudianteDto estudianteDto);
        Task ActualizarAsync(EstudianteDto estudianteDto);
        Task EliminarAsync(Guid estudianteId);
        Task<ObjectResultDto<List<EstudianteSelectDto>>> GetEstudiantesForSelectAsync();
    }
}
