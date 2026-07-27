using EduApoyosApplication.Implementation;
using EduApoyosDomain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduApoyosWebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthApplication _authApplication;

        public AuthController(IAuthApplication authApplication)
        {
            _authApplication = authApplication;
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginUsuarioDto loginUsuarioDto)
        {
            var token = await _authApplication.LoginAsync(loginUsuarioDto);
            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync([FromBody] CrearUsuarioDto crearUsuarioDto)
        {
            await _authApplication.CrearUsuarioAsync(crearUsuarioDto);
            return Ok();
        }
    }
}
