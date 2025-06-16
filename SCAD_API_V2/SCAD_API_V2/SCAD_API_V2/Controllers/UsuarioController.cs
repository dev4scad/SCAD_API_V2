using Microsoft.AspNetCore.Mvc;
using SCAD_API_V2.Application.Interfaces;
using SCAD_API_V2.Application.DTO;

namespace SCAD_API_V2.Controllers
{
    [ApiController]
    [Route("api/licenca/v2/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioServices _usuarioService;

        public UsuarioController(IUsuarioServices usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> ListarUsuarios()
        {
            var result = await _usuarioService.ListarUsuariosAsync();
            return Ok(result);
        }

        [HttpGet("{usuarioId:int}")]
        public async Task<IActionResult> BuscarUsuarioPorId(int usuarioId)
        {
            var result = await _usuarioService.BuscarUsuarioPorIdAsync(usuarioId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> BuscarUsuarioPorEmail(string email)
        {
            var result = await _usuarioService.BuscarUsuarioPorEmailAsync(email);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> BuscarUsuarioPorNome(string nome)
        {
            var result = await _usuarioService.BuscarUsuarioPorNomeAsync(nome);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarUsuario([FromBody] UsuarioDto usuarioDto)
        {
            var result = await _usuarioService.CadastrarUsuarioAsync(usuarioDto);
            return Created("", result);
        }

        [HttpPut]
        public async Task<IActionResult> EditarUsuario([FromBody] UsuarioDto usuarioDto)
        {
            var result = await _usuarioService.EditarUsuarioAsync(usuarioDto);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{usuarioId:int}")]
        public async Task<IActionResult> ExcluirUsuario(int usuarioId)
        {
            var result = await _usuarioService.ExcluirUsuarioAsync(usuarioId);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
