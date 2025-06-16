using SCAD_API_V2.Domain.Entities;

namespace SCAD_API_V2.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> BuscarClientesAsync();
        Task<Cliente> BuscarClientePorIdAsync(int usuarioId);
        Task<Cliente> BuscarClientePorCNPJ_CPFAsync(string CNPJ_CPF);
        Task<Cliente> BuscarClientePorEmailAsync(string email);
        Task<List<Cliente>> BuscarClientePorNomeAsync(string nome);
        Task<List<Cliente>> BuscarClientePorTelefoneAsync(string telefone);
        Task<List<Cliente>> CriarClienteAsync(Cliente clienteDto);
        Task<Cliente> EditarClienteAsync(Cliente clienteDto);
        Task<bool> ExcluirClienteAsync(int usuarioId);
    }
}
