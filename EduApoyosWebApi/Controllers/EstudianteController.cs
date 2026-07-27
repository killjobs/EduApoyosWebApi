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
        public async Task<ActionResult<ObjectResultDto<Estudiante>>> GetEstudiantesAsync()
        {
            var result = await _estudianteApplication.GetEstudiantesAsync();
            return Ok(result);
        }
    }
}
