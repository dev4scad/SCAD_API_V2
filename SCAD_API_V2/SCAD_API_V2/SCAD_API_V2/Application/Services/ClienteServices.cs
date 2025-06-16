using AutoMapper;
using SCAD_API_V2.Application.DTO;
using SCAD_API_V2.Application.Interfaces;
using SCAD_API_V2.Domain.Entities;
using SCAD_API_V2.Domain.Interfaces;

namespace SCAD_API_V2.Application.Services
{
    public class ClienteServices : IClienteServices
    {
        private readonly IClienteRepository _repo;
        private readonly IMapper _mapper;

        public ClienteServices(IClienteRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<ClienteDto>> BuscarClientesAsync()
        {
            var entidades = await _repo.BuscarClientesAsync();
            return _mapper.Map<List<ClienteDto>>(entidades);
        }

        public async Task<ClienteDto> BuscarClientePorIdAsync(int usuarioId)
        {
            var entidade = await _repo.BuscarClientePorIdAsync(usuarioId);
            return _mapper.Map<ClienteDto>(entidade);
        }

        public async Task<ClienteDto> BuscarClientePorCNPJ_CPFAsync(string CNPJ_CPF)
        {
            var entidade = await _repo.BuscarClientePorCNPJ_CPFAsync(CNPJ_CPF);
            return _mapper.Map<ClienteDto>(entidade);
        }

        public async Task<ClienteDto> BuscarClientePorEmailAsync(string email)
        {
            var entidade = await _repo.BuscarClientePorEmailAsync(email);
            return _mapper.Map<ClienteDto>(entidade);
        }

        public async Task<List<ClienteDto>> BuscarClientePorNomeAsync(string nome)
        {
            var lista = await _repo.BuscarClientePorNomeAsync(nome);
            return _mapper.Map<List<ClienteDto>>(lista);
        }

        public async Task<List<ClienteDto>> BuscarClientePorTelefoneAsync(string telefone)
        {
            var lista = await _repo.BuscarClientePorTelefoneAsync(telefone);
            return _mapper.Map<List<ClienteDto>>(lista);
        }

        public async Task<List<ClienteDto>> CriarClienteAsync(ClienteDto clienteDto)
        {
            var entidade = _mapper.Map<Cliente>(clienteDto);
            var criados = await _repo.CriarClienteAsync(entidade);
            return _mapper.Map<List<ClienteDto>>(criados);
        }

        public async Task<ClienteDto> EditarClienteAsync(ClienteDto clienteDto)
        {
            var entidade = _mapper.Map<Cliente>(clienteDto);
            var editado = await _repo.EditarClienteAsync(entidade);
            return _mapper.Map<ClienteDto>(editado);
        }

        public async Task<bool> ExcluirClienteAsync(int usuarioId)
        {
            return await _repo.ExcluirClienteAsync(usuarioId);
        }
    }
}
