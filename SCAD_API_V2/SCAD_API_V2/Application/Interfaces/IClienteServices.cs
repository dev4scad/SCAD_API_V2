using SCAD_API_V2.Application.DTO;

namespace SCAD_API_V2.Application.Interfaces
{
    public interface IClienteServices
    {
        Task<List<ClienteDto>> BuscarUsuariosAsync();
        Task<ClienteDto> BuscarUsuarioPorIdAsync(int usuarioId);
        Task<ClienteDto> BuscarUsuarioPorCPF_CNPJAsync(string CPF_CNPJ);
        Task<ClienteDto> BuscarUsuarioPorEmailAsync(string email);
        Task<List<ClienteDto>> BuscarUsuarioPorNomeAsync(string nome);
        Task<List<ClienteDto>> BuscarUsuarioPorTelefoneAsync(string telefone);
        Task<List<ClienteDto>> CriarUsuarioAsync(ClienteDto clienteDto);
        Task<ClienteDto> EditarUsuarioAsync(ClienteDto clienteDto);
        Task<bool> ExcluirUsuarioAsync(int usuarioId);
    }
}
