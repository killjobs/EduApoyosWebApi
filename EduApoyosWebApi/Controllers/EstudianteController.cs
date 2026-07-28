using EduApoyosApplication.Implementation;
using EduApoyosDomain.Dtos;
using EduApoyosDomain.Entities;
using EduApoyosDomain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduApoyosWebApi.Controllers
{
    [ApiController]
    [Route("api/estudiantes")]
    public class EstudianteController : ControllerBase
    {
        private readonly IEstudianteApplication _estudianteApplication;

        public EstudianteController(IEstudianteApplication estudianteApplication)
        {
            _estudianteApplication = estudianteApplication;
        }

        [Authorize(Roles = nameof(RolUsuario.Asesor))]
        [HttpGet]
        public async Task<ActionResult<ObjectResultDto<Estudiante>>> GetEstudiantesAsync(int page = 1,int pageSize = 10)
        {
            var result = await _estudianteApplication.GetEstudiantesAsync(page, pageSize);
            return Ok(result);
        }

        [Authorize(Roles = nameof(RolUsuario.Asesor))]
        [HttpPost]
        public async Task<ActionResult<ObjectResultDto<string>>> CrearAsync([FromBody] EstudianteDto estudianteDto)
        {
            await _estudianteApplication.CrearAsync(estudianteDto);

            return Ok(new ObjectResultDto<string>
            {
                Success = true,
                Data = "Estudiante creado correctamente."
            });
        }
    }
}
