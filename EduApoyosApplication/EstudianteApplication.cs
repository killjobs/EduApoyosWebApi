using EduApoyosApplication.Implementation;
using EduApoyosCommon.Interface;
using EduApoyosDomain.Dtos;
using EduApoyosDomain.Entities;

namespace EduApoyosApplication
{
    public class EstudianteApplication : IEstudianteApplication
    {
        private readonly IEstudianteRepository _estudianteRepository;

        public EstudianteApplication(IEstudianteRepository estudianteRepository)
        {
            _estudianteRepository = estudianteRepository;
        }
        public async Task<ObjectResultDto<List<Estudiante>>> GetEstudiantesAsync()
        {
            return new ObjectResultDto<List<Estudiante>>
            {
                Data = await _estudianteRepository.GetEstudiantesAsync(),
                Success = true
            };
        }
    }
}
