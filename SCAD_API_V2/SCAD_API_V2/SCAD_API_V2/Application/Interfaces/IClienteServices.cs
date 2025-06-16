using SCAD_API_V2.Application.DTO;

namespace SCAD_API_V2.Application.Interfaces
{
    public interface IClienteServices
    {
        Task<List<ClienteDto>> BuscarClientesAsync();
        Task<ClienteDto> BuscarClientePorIdAsync(int ClienteId);
        Task<ClienteDto> BuscarClientePorCNPJ_CPFAsync(string CNPJ_CPF);
        Task<ClienteDto> BuscarClientePorEmailAsync(string email);
        Task<List<ClienteDto>> BuscarClientePorNomeAsync(string nome);
        Task<List<ClienteDto>> BuscarClientePorTelefoneAsync(string telefone);
        Task<List<ClienteDto>> CriarClienteAsync(ClienteDto clienteDto);
        Task<ClienteDto> EditarClienteAsync(ClienteDto clienteDto);
        Task<bool> ExcluirClienteAsync(int ClienteId);
    }
}
