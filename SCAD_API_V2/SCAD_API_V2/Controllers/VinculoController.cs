using Microsoft.AspNetCore.Mvc;
using SCAD_API_V2.Application.Interfaces;
using SCAD_API_V2.Application.DTO;

namespace SCAD_API_V2.Controllers
{
    [ApiController]
    [Route("api/licenca/v2/{banco}/vinculo")]
    public class VinculoController : ControllerBase
    {
        private readonly IVinculoServices _vinculoService;

        public VinculoController(IVinculoServices vinculoService)
        {
            _vinculoService = vinculoService;
        }

        [HttpGet]
        public async Task<IActionResult> ListarVinculos()
        {
            var result = await _vinculoService.ListarVinculosAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> BuscarVinculoPorId(int id)
        {
            var result = await _vinculoService.BuscarVinculoPorIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("licenca/{licenca}")]
        public async Task<IActionResult> BuscarVinculoPorLicenca(string licenca)
        {
            var result = await _vinculoService.BuscarVinculoPorLicencaAsync(licenca);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("maquina/{maquina}")]
        public async Task<IActionResult> BuscarVinculoPorMaquina(string maquina)
        {
            var result = await _vinculoService.BuscarVinculoPorMaquinaAsync(maquina);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("licencaId/{licencaId:int}")]
        public async Task<IActionResult> BuscarVinculoPorLicencaId(int licencaId)
        {
            var result = await _vinculoService.BuscarVinculoPorLicencaIdAsync(licencaId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("nomeMaquina/{nomeMaquina}")]
        public async Task<IActionResult> BuscarVinculoPorNomeMaquina(string nomeMaquina)
        {
            var result = await _vinculoService.BuscarVinculoPorNomeMaquinaAsync(nomeMaquina);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarVinculo([FromBody] VinculoDto vinculoDto)
        {
            var result = await _vinculoService.CadastrarVinculoAsync(vinculoDto);
            return Created("", result);
        }

        [HttpPut]
        public async Task<IActionResult> EditarVinculo([FromBody] VinculoDto vinculoDto)
        {
            var result = await _vinculoService.EditarVinculoAsync(vinculoDto);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> ExcluirVinculo(int id)
        {
            var result = await _vinculoService.ExcluirVinculoAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
