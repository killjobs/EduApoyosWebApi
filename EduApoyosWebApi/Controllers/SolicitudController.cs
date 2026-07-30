using EduApoyosApplication.Implementation;
using EduApoyosDomain.Dtos;
using EduApoyosDomain.Entities;
using EduApoyosDomain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EduApoyosWebApi.Controllers
{
    [ApiController]
    [Route("api/solicitudes")]
    public class SolicitudController : ControllerBase
    {
        private readonly ISolicitudApplication _solicitudApplication;

        public SolicitudController(ISolicitudApplication solicitudApplication)
        {
            _solicitudApplication = solicitudApplication;
        }

        [Authorize(Roles = nameof(RolUsuario.Asesor))]
        [HttpGet]
        public async Task<ActionResult<ObjectResultDto<SolicitudApoyo>>> GetSolicitudesAsync(int page = 1, int pageSize = 10,EstadoSolicitudEnum? estado = null)
        {
            var result = await _solicitudApplication.GetSolicitudesAsync(page, pageSize,estado);
            return Ok(result);
        }

        [Authorize(Roles = $"{nameof(RolUsuario.Asesor)},{nameof(RolUsuario.Estudiante)}")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ObjectResultDto<SolicitudApoyo>>> GetByIdHistorialAsync(Guid id)
        {
            var usuarioIdClaim =User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var rolClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(usuarioIdClaim, out var usuarioId))
            {
                throw new UnauthorizedAccessException("Usuario no válido.");
            }
            if (!Enum.TryParse<RolUsuario>(rolClaim, out var rol))
            {
                throw new UnauthorizedAccessException("Rol inválido.");
            }

            var result = await _solicitudApplication.GetByIdHistorialAsync(id, usuarioId, rol);
            return Ok(result);
        }

        [Authorize(Roles = $"{nameof(RolUsuario.Asesor)},{nameof(RolUsuario.Estudiante)}")]
        [HttpPost]
        public async Task<ActionResult<ObjectResultDto<string>>> CrearAsync([FromBody] CrearSolicitudApoyoDto dto)
        {
            await _solicitudApplication.CrearAsync(dto);
            return Ok(new ObjectResultDto<string>
            {
                Success = true,
                Data = "Solicitud creada correctamente."
            });
        }

        [Authorize(Roles = nameof(RolUsuario.Asesor))]
        [HttpPatch("{id:guid}/estado")]
        public async Task<IActionResult> CambiarEstadoAsync(Guid id, [FromBody] CambiarEstadoSolicitudDto solicitudDto)
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(usuarioIdClaim, out var usuarioId))
            {
                throw new UnauthorizedAccessException("Usuario no válido.");
            }

            await _solicitudApplication.CambiarEstadoAsync(id, solicitudDto, usuarioId);
            return Ok(new ObjectResultDto<string>
            {
                Success = true,
                Data = "Solicitud actualizada correctamente."
            });
        }
    }
}
