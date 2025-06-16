using SCAD_API_V2.Domain.Entities;

namespace SCAD_API_V2.Application.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> ListarUsuariosAsync();
        Task<Usuario> BuscarUsuarioPorIdAsync(int usuarioId);
        Task<Usuario> BuscarUsuarioPorEmailAsync(string email);
        Task<Usuario> BuscarUsuarioPorNomeAsync(string nome);
        Task<Usuario> CadastrarUsuarioAsync(Usuario usuarioDto);
        Task<Usuario> EditarUsuarioAsync(Usuario usuarioDto);
        Task<bool> ExcluirUsuarioAsync(int usuarioId);
    }
}
