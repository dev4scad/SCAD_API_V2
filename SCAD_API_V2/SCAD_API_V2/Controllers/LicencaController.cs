using Microsoft.AspNetCore.Mvc;
using SCAD_API_V2.Application.Interfaces;
using SCAD_API_V2.Application.DTO;

namespace SCAD_API_V2.Controllers
{
    [ApiController]
    [Route("api/licenca/v2/{banco}/licenca")]
    public class LicencaController : ControllerBase
    {
        private readonly ILicencaServices _licencaService;

        public LicencaController(ILicencaServices licencaService)
        {
            _licencaService = licencaService;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarLicencas()
        {
            var result = await _licencaService.BuscarLicencasAsync();
            return Ok(result);
        }

        [HttpGet("{licencaId:int}")]
        public async Task<IActionResult> BuscarLicencaPorId(int licencaId)
        {
            var result = await _licencaService.BuscarLicencaPorIdAsync(licencaId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("key/{licencaKey}")]
        public async Task<IActionResult> BuscarLicencaPorKey(string licencaKey)
        {
            var result = await _licencaService.BuscarLicencaPorKeyAsync(licencaKey);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("cliente/{clienteId:int}")]
        public async Task<IActionResult> BuscarLicencaPorClienteId(int clienteId)
        {
            var result = await _licencaService.BuscarLicencaPorClienteIdAsync(clienteId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CriarLicenca([FromBody] LicencaDto licencaDto)
        {
            var result = await _licencaService.CriarLicencaAsync(licencaDto);
            return Created("", result);
        }

        [HttpPut]
        public async Task<IActionResult> EditarLicenca([FromBody] LicencaDto licencaDto)
        {
            var result = await _licencaService.EditarLicencaAsync(licencaDto);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{licencaId:int}")]
        public async Task<IActionResult> ExcluirLicenca(int licencaId)
        {
            var result = await _licencaService.ExcluirLicencaAsync(licencaId);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
