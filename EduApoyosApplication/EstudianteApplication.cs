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
        public async Task<ObjectResultDto<PagedResultDto<Estudiante>>> GetEstudiantesAsync(int page, int pageSize)
        {
            var estudiantes = await _estudianteRepository.GetEstudiantesAsync(page,pageSize);
            var totalRecords =await _estudianteRepository.CountEstudiantesAsync();

            var totalPages =(int)Math.Ceiling(totalRecords /(double)pageSize);

            return new ObjectResultDto<PagedResultDto<Estudiante>>
            {
                Success = true,
                Data = new PagedResultDto<Estudiante>
                {
                    Items = estudiantes,
                    Page = page,
                    PageSize = pageSize,
                    TotalRecords = totalRecords,
                    TotalPages = totalPages
                }
            };
        }
        public async Task<ObjectResultDto<Estudiante>> GetEstudianteByIdAsync(Guid id)
        {
            return new ObjectResultDto<Estudiante>
            {
                Data = await _estudianteRepository.GetEstudianteByIdAsync(id),
                Success = true
            };
        }
        public async Task<ObjectResultDto<Estudiante>> GetEstudianteByNumeroDocumentoAsync(string numeroDocumento)
        {
            return new ObjectResultDto<Estudiante>
            {
                Data = await _estudianteRepository.GetEstudianteByNumeroDocumentoAsync(numeroDocumento),
                Success = true
            };
        }
        public async Task CrearAsync(EstudianteDto estudianteDto)
        {
            var existeEstudiante = await _estudianteRepository.GetEstudianteByNumeroDocumentoAsync(estudianteDto.NumeroDocumento);
            if (existeEstudiante != null)
            {
                throw new InvalidOperationException($"El estudiante con número de documento {estudianteDto.NumeroDocumento} ya existe.");
            }

            var estudiante = new Estudiante()
            {
                Id = Guid.NewGuid(),
                UsuarioId = estudianteDto.UsuarioId,
                NumeroDocumento = estudianteDto.NumeroDocumento,
                TipoDocumento = estudianteDto.TipoDocumento,
                ProgramaAcademico = estudianteDto.ProgramaAcademico,
                Semestre = estudianteDto.Semestre,
            };
            await _estudianteRepository.CrearAsync(estudiante);
            await _estudianteRepository.GuardarCambiosAsync();
        }
        public async Task ActualizarAsync(EstudianteDto estudianteDto)
        {
            if (!estudianteDto.Id.HasValue)
            {
                throw new InvalidOperationException("El Id del estudiante es requerido.");
            }

            var estudiante = await _estudianteRepository.GetEstudianteByIdAsync(estudianteDto.Id.Value);
            if (estudiante == null)
            {
                throw new InvalidOperationException($"El estudiante con número de documento {estudianteDto.NumeroDocumento} no existe.");
            }
            estudiante.NumeroDocumento = estudianteDto.NumeroDocumento;
            estudiante.TipoDocumento = estudianteDto.TipoDocumento;
            estudiante.ProgramaAcademico = estudianteDto.ProgramaAcademico;
            estudiante.Semestre = estudianteDto.Semestre;

            await _estudianteRepository.GuardarCambiosAsync();
        }
        public async Task EliminarAsync(Guid id)
        {
            var estudiante = await _estudianteRepository.GetEstudianteByIdAsync(id);
            if (estudiante == null)
            {
                throw new InvalidOperationException($"El estudiante con id {id} no existe.");
            }

            await _estudianteRepository.EliminarAsync(estudiante);
            await _estudianteRepository.GuardarCambiosAsync();
        }
        public async Task<ObjectResultDto<List<EstudianteSelectDto>>> GetEstudiantesForSelectAsync()
        {
            return new ObjectResultDto<List<EstudianteSelectDto>>
            {
                Data = await _estudianteRepository.GetEstudiantesForSelectAsync(),
                Success = true
            };
        }
    }
}
