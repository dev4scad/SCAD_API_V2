using SCAD_API_V2.Domain.Entities;

namespace SCAD_API_V2.Domain.Interfaces
{
    public interface IClienteInterface
    {
        Task<List<Cliente>> BuscarUsuariosAsync();
        Task<Cliente> BuscarUsuarioPorIdAsync(int usuarioId);
        Task<Cliente> BuscarUsuarioPorCPF_CNPJAsync(string CPF_CNPJ);
        Task<Cliente> BuscarUsuarioPorEmailAsync(string email);
        Task<List<Cliente>> BuscarUsuarioPorNomeAsync(string nome);
        Task<List<Cliente>> BuscarUsuarioPorTelefoneAsync(string telefone);
        Task<List<Cliente>> CriarUsuarioAsync(Cliente clienteDto);
        Task<Cliente> EditarUsuarioAsync(Cliente clienteDto);
        Task<bool> ExcluirUsuarioAsync(int usuarioId);
    }
}
