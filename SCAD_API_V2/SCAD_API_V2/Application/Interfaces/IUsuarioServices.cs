using SCAD_API_V2.Application.DTO;

namespace SCAD_API_V2.Application.Interfaces
{
    public interface IUsuarioServices
    {
        Task<List<UsuarioDto>> ListarUsuariosAsync();
        Task<UsuarioDto> BuscarUsuarioPorIdAsync(int usuarioId);
        Task<UsuarioDto> BuscarUsuarioPorEmailAsync(string email);
        Task<UsuarioDto> BuscarUsuarioPorNomeAsync(string nome);
        Task<UsuarioDto> CadastrarUsuarioAsync(UsuarioDto usuarioDto);
        Task<UsuarioDto> EditarUsuarioAsync(UsuarioDto usuarioDto);
        Task<bool> ExcluirUsuarioAsync(int usuarioId);
    }
}
