using Microsoft.AspNetCore.Mvc;
using SCAD_API_V2.Application.Interfaces;
using SCAD_API_V2.Application.DTO;

namespace SCAD_API_V2.Controllers
{
    [ApiController]
    [Route("api/licenca/v2/{banco}/cliente")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteServices _clienteService;

        public ClienteController(IClienteServices clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarClientes()
        {
            var result = await _clienteService.BuscarClientesAsync();
            return Ok(result);
        }

        [HttpGet("{usuarioId:int}")]
        public async Task<IActionResult> BuscarClientePorId(int usuarioId)
        {
            var result = await _clienteService.BuscarClientePorIdAsync(usuarioId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("cpf/{cpfCnpj}")]
        public async Task<IActionResult> BuscarClientePorCNPJ_CPF(string cpfCnpj)
        {
            var result = await _clienteService.BuscarClientePorCNPJ_CPFAsync(cpfCnpj);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> BuscarClientePorEmail(string email)
        {
            var result = await _clienteService.BuscarClientePorEmailAsync(email);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("nome/{nome}")]
        public async Task<IActionResult> BuscarClientePorNome(string nome)
        {
            var result = await _clienteService.BuscarClientePorNomeAsync(nome);
            return Ok(result);
        }

        [HttpGet("telefone/{telefone}")]
        public async Task<IActionResult> BuscarClientePorTelefone(string telefone)
        {
            var result = await _clienteService.BuscarClientePorTelefoneAsync(telefone);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CriarCliente([FromBody] ClienteDto clienteDto)
        {
            var result = await _clienteService.CriarClienteAsync(clienteDto);
            if (result == null || result.Count == 0)
                return Conflict("Cliente já cadastrado com este CNPJ/CPF.");
            return Created("", result);
        }

        [HttpPut]
        public async Task<IActionResult> EditarCliente([FromBody] ClienteDto clienteDto)
        {
            var result = await _clienteService.EditarClienteAsync(clienteDto);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{usuarioId:int}")]
        public async Task<IActionResult> ExcluirCliente(int usuarioId)
        {
            var result = await _clienteService.ExcluirClienteAsync(usuarioId);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
